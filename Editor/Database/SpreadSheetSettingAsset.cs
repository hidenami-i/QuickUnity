#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using QuickUnity.Editor.Database.TableDefinition;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Runtime.Database;
using QuickUnity.Utility;
using UnityEditor;
using UnityEngine;

namespace IMDB4Unity.Editor
{
    [Serializable]
    public class SpreadSheetInfo
    {
        [SerializeField] private string name = "";
        [SerializeField] private string spreadSheetId = "";

        public string Name => name;
        public string SpreadSheetId => spreadSheetId;
    }

    [Serializable]
    public class SheetInfo
    {
        [SerializeField] private string title;
        [SerializeField] private string spreadSheetId;

        public SheetInfo(Sheet sheet, string spreadSheetId)
        {
            title = sheet.Properties.Title;
            this.spreadSheetId = spreadSheetId;
        }

        public string Title => title;
        public string SpreadSheetId => spreadSheetId;
    }

    public class SpreadSheetSettingAsset : ScriptableObject
    {
        static readonly string[] Scopes = {SheetsService.Scope.SpreadsheetsReadonly};
        private const string ApplicationName = "IMDB for unity";

        [SerializeField] private TextAsset spreadSheetCredentialJson;

        private bool IsNullCredential => spreadSheetCredentialJson == null;

        public List<SpreadSheetInfo> SpreadSheetInfoList = new List<SpreadSheetInfo>();

        public SheetsService GetSheetService()
        {
            if (IsNullCredential)
            {
                throw new ArgumentNullException($"credetial.json is null.");
            }

            var path = AssetDatabase.GetAssetPath(spreadSheetCredentialJson);
            UserCredential credential;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                var credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                Debug.Log("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential, ApplicationName = ApplicationName
            });
        }

        public void UpdateMasterData(SheetInfo sheetInfo)
        {
            EditorUtility.DisplayProgressBar(sheetInfo.Title, "", 0);
            ValueRange result = GetSheetService().Spreadsheets.Values.Get(sheetInfo.SpreadSheetId, sheetInfo.Title)
                .Execute();
            var sheetValues = result.Values;

            var repositoryName = "Master" + sheetInfo.Title.Replace("[D]", "").ConvertsSnakeToUpperCamel() +
                                 "Repository";

            Debug.Log($"repository name : {repositoryName}");

            Type repositoryType = repositoryName.ToType();

            Debug.Log($"repository type : {repositoryType.Name}");

            IList<string> fieldKeys = sheetValues[1].Select(x => x.ToString()).ToList();
            var values = new List<Dictionary<string, object>>();

            for (var index = 3; index < sheetValues.Count; index++)
            {
                var row = sheetValues[index];
                var dictionary = new Dictionary<string, object>();
                for (var i = 0; i < fieldKeys.Count; i++)
                {
                    var key = fieldKeys[i];
                    object value = row.ElementAtOrDefault(i);
                    if (string.IsNullOrEmpty(key) || value == null)
                    {
                        continue;
                    }

                    dictionary.Add(key, value);
                }

                values.Add(dictionary);
            }

            try
            {
                object repositoryInstance = Activator.CreateInstance(repositoryType);
                DatabaseReflection.InsertAll(repositoryType, repositoryInstance, values);

                var json = JsonUtility.ToJson(repositoryInstance, true);
                IDatabase database = repositoryInstance as IDatabase;
                File.WriteAllText(
                    Path.Combine(Application.dataPath, "../", DatabaseSettings.Location.FolderName, database.Schema,
                        database.KName + ".json"), json);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                throw;
            }
            finally
            {
                RuntimeUnityEditor.AssetDataBaseRefresh();
                EditorUtility.ClearProgressBar();
            }
        }

        public void UpdateAllMasterData(List<SheetInfo> sheetInfoList)
        {
            foreach (SheetInfo sheetInfo in sheetInfoList)
            {
                UpdateMasterData(sheetInfo);
            }
        }

        public async void GenerateScriptAsync(SpreadSheetInfo spreadSheetInfo)
        {
            Spreadsheet spreadsheet =
                await GetSheetService().Spreadsheets.Get(spreadSheetInfo.SpreadSheetId).ExecuteAsync();
            var enumSheetList = spreadsheet.Sheets.Where(x => x.Properties.Title.Contains("[E]")).ToList();

            for (var index = 0; index < enumSheetList.Count; index++)
            {
                Sheet sheet = enumSheetList[index];
                var sheetTitle = sheet.Properties.Title;
                Debug.Log($"sheet title {sheetTitle}");

                var progress = (float) index / enumSheetList.Count;
                ValueRange resultValues = await GetSheetService().Spreadsheets.Values
                    .Get(spreadSheetInfo.SpreadSheetId, sheetTitle)
                    .ExecuteAsync();

                EditorUtility.DisplayProgressBar(sheet.Properties.Title, "", progress);

                var sheetValues = resultValues.Values;
                var logicalName = sheetValues[1][2].ToString();
                var physicalName = sheetValues[2][2].ToString();

                var enumDataEntityList = CreateEnumDataEntityList(sheetValues);
                EnumTableEntity enumTableEntity = new EnumTableEntity(SchemaType.Enumeration.ToString(), logicalName,
                    physicalName,
                    enumDataEntityList);
                EnumTableRepository.Me.Add(enumTableEntity);
            }

            var list = EnumTableRepository.Me.FindAll();
            for (var index = 0; index < list.Count; index++)
            {
                EnumTableEntity entity = list[index];

                var progress = (float) index / list.Count;
                EditorUtility.DisplayProgressBar("Generate Enumeration...", entity.EnumPhysicalName, progress);

                var contents = entity.GenerateEnumScript();
                const string folderPath = "Assets/Scripts/Database/auto_generated/Enumerations";
                var enumFilePath = Path.Combine(folderPath, entity.ScriptFileName);
                ExIO.CreateDirectoryNotExist(folderPath);
                ExIO.WriteAllTextAsUTF8(enumFilePath, contents);
                RuntimeUnityEditor.AssetDataBaseRefresh();

                string[] labels = {SchemaType.Enumeration.ToString()};
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<TextAsset>(enumFilePath), labels);
            }

            AssetDatabase.Refresh();

            //
            // var tList = result.Sheets.Where(x => x.Properties.Title.Contains("[T]")).ToList();
            //
            // foreach (Sheet sheet in tList)
            // {
            //     string range = sheet.Properties.Title;
            //     var resultValues = await GetSheetService().Spreadsheets.Values.Get(spreadSheetInfo.SpreadSheetId, range)
            //         .ExecuteAsync();
            //     EditorUtility.DisplayProgressBar(sheet.Properties.Title, "", 0);
            //     IList<IList<object>> sheetValues = resultValues.Values;
            //     string schema = sheetValues[0][2].ToString();
            //     string logicalName = sheetValues[1][2].ToString();
            //     string physicalName = sheetValues[2][2].ToString();
            //     string persistentObjectType = sheetValues[3][2].ToString();
            //     List<TableDefinitionDataEntity> tableDefinitionDataEntities =
            //         ToTableDefinitionDataEntityList(sheetValues);
            //     TableDefinitionEntity entity = new TableDefinitionEntity(schema, logicalName, physicalName,
            //         persistentObjectType, tableDefinitionDataEntities);
            //     TableDefinitionRepository.Me.Add(entity);
            // }
            //
            // GenerateTableDefinitionScript();

            EditorUtility.ClearProgressBar();
        }

        void GenerateTableDefinitionScript()
        {
            var list =
                TableDefinitionRepository.Me.FindAllBy(x => !x.SchemaType.IsEnumeration());

            for (var index = 0; index < list.Count; index++)
            {
                TableDefinitionEntity entity = list[index];

                var progress = (float) index / list.Count;
                EditorUtility.DisplayProgressBar("Generate TableDefinition...", entity.PhysicalName, progress);

                var entityScript = entity.GenerateEntityScript();
                var entityFilePath = Path.Combine("Assets/App/Database", entity.SchemaType.ToString(),
                    entity.GetEntityScriptFileName);
                File.WriteAllText(entityFilePath, entityScript);

                var entityServiceScript = entity.GenerateEntityServiceScript();
                var entityServiceFilePath = Path.Combine("Assets/App/Database/", entity.SchemaType.ToString(),
                    entity.GetEntityServiceScriptFileName);
                if (!File.Exists(entityServiceFilePath))
                {
                    File.WriteAllText(entityServiceFilePath, entityServiceScript);
                }

                var repositoryScript = "";
                var repositoryServiceScript = "";
                var fileName = "";
                var serviceFilePath = "";

                if (entity.PersistentObjectType.IsRepository())
                {
                    repositoryScript = entity.GenerateRepositoryScript();
                    fileName = entity.GetRepositoryScriptFileName;

                    repositoryServiceScript = entity.GenerateRepositoryServiceScript();
                    serviceFilePath = Path.Combine("Assets/App/Database/", entity.SchemaType.ToString(),
                        entity.GetRepositoryServiceScriptFileName);
                }
                else if (entity.PersistentObjectType.IsDataMapper())
                {
                    repositoryScript = entity.GenerateDataMapperScript();
                    fileName = entity.GetDataMapperScriptFileName;

                    repositoryServiceScript = entity.GenerateDataMapperServiceScript();
                    serviceFilePath = Path.Combine("Assets/App/Database/", entity.SchemaType.ToString(),
                        entity.GetDataMapperServiceScriptFileName);
                }

                var repositoryFilePath =
                    Path.Combine("Assets/App/Database/", entity.SchemaType.ToString(), fileName);
                File.WriteAllText(repositoryFilePath, repositoryScript);

                if (!File.Exists(serviceFilePath))
                {
                    File.WriteAllText(serviceFilePath, repositoryServiceScript);
                }

                RuntimeUnityEditor.AssetDataBaseRefresh();

                string[] entityLabels = {entity.SchemaType.ToString(), "Entity", "Database"};
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<TextAsset>(entityFilePath), entityLabels);

                string[] repositoryLabels =
                    {entity.SchemaType.ToString(), entity.PersistentObjectType.ToString(), "Database"};
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<TextAsset>(repositoryFilePath), repositoryLabels);
            }
        }

        List<TableDefinitionDataEntity> ToTableDefinitionDataEntityList(IList<IList<object>> sheetValues)
        {
            var result = new List<TableDefinitionDataEntity>();
            for (var i = 7; i < sheetValues.Count; i++)
            {
                var row = sheetValues[i];
                var logicalName = row.ElementAtOrDefault(1)?.ToString();
                var physicalName = row.ElementAtOrDefault(2)?.ToString();
                var dataType = row.ElementAtOrDefault(3)?.ToString();
                var defaultValue = row.ElementAtOrDefault(7)?.ToString();
                var relation = row.ElementAtOrDefault(8)?.ToString();

                // undefined dataType is threw
                if (string.IsNullOrEmpty(dataType))
                {
                    continue;
                }

                TableDefinitionDataEntity entity =
                    new TableDefinitionDataEntity(logicalName, physicalName, dataType, defaultValue, relation);
                result.Add(entity);
            }

            return result;
        }

        private List<EnumDataEntity> CreateEnumDataEntityList(IList<IList<object>> sheetValues)
        {
            const int DataStartIndex = 5;
            var result = new List<EnumDataEntity>();
            for (var i = DataStartIndex; i < sheetValues.Count; i++)
            {
                var row = sheetValues[i];
                var index = row.ElementAtOrDefault(0).ToInt(0);
                var logicalName = row.ElementAtOrDefault(1)?.ToString();
                var physicalName = row.ElementAtOrDefault(2)?.ToString();
                var value = row.ElementAtOrDefault(3).ToInt();
                var description = row.ElementAtOrDefault(4)?.ToString();

                if (index.IsZero() || index.IsNegative())
                {
                    continue;
                }

                EnumDataEntity entity =
                    new EnumDataEntity(index, logicalName, physicalName, value, description);
                result.Add(entity);
            }

            return result;
        }
    }
}

#endif