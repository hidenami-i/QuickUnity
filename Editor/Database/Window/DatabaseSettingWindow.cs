using QuickUnity.Editor.Extensions;
using QuickUnity.Extensions.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using QuickUnity.Editor.Database.Entities;
using QuickUnity.Editor.Database.Enumerations;
using QuickUnity.Editor.Database.TableDefinition;
using QuickUnity.Utility;

namespace QuickUnity.Editor.Database.Window
{
    public class DatabaseSettingWindow : EditorWindow
    {
        private static readonly DatabaseSettingWindow Window = null;
        private SheetsService sheetsService;
        private SpreadSheetSettingAsset spreadSheetSettingAsset;
        private Vector2 scrollPos = Vector2.zero;
        private List<SheetInfoEntity> sheetInfoList;
        private const string ScriptAutoGeneratedSaveLocation = "Assets/App/auto_generated/Scripts/Database";
        private const string ScriptSaveLocation = "Assets/App/Scripts/Database";
        private const string ScriptNameSpace = "App.Database";

        [MenuItem(itemName: "QuickUnity/Database/DatabaseSettingWindow")]
        static void ShowWindow()
        {
            if (Window != null)
            {
                Window.Close();
            }

            GetWindow<DatabaseSettingWindow>(title: nameof(DatabaseSettingWindow)).Show();
        }

        private void OnEnable()
        {
            SetupSpreadSheetSettingAsset();
        }

        private void OnGUI()
        {
            ExEditorGUI.Space();

            EditorGUILayout.ObjectField(label: nameof(SpreadSheetSettingAsset),
                obj: spreadSheetSettingAsset,
                objType: typeof(SpreadSheetSettingAsset), allowSceneObjects: false);

            if (spreadSheetSettingAsset == null)
            {
                ExEditorGUI.HelpErrorBox(message: "Spread Sheet Setting Asset is null.");
                return;
            }

            ExEditorGUI.Space();
            ExEditorGUI.LabelFieldWithLabelField(label1: "Script Save Location",
                label2: ScriptSaveLocation);

            ExEditorGUI.Space();
            ExEditorGUI.LabelFieldWithLabelField(label1: "Script Auto Generated Save Location",
                label2: ScriptAutoGeneratedSaveLocation);

            ExEditorGUI.Space();
            ExEditorGUI.LabelFieldWithLabelField(label1: "Script Name Space", label2: ScriptNameSpace);

            ExEditorGUI.Space();
            ExEditorGUI.Space();
            ExEditorGUI.BeginDisabledGroupUnityEditorBusy();
            ExEditorGUI.LabelFieldAsBold(label: "Generate Table Definition All Scripts");

            foreach (SpreadSheetInfoEntity spreadSheetInfo in spreadSheetSettingAsset.SpreadSheetInfoList)
            {
                ExEditorGUI.Space();
                ExEditorGUI.LabelFieldWithLabelField(label1: "Name", label2: spreadSheetInfo.Name);
                ExEditorGUI.LabelFieldWithLabelField(label1: "Spread Sheet Id", label2: spreadSheetInfo.SpreadSheetId);
                ExEditorGUI.Space();

                EditorGUILayout.BeginHorizontal();

                if (ExEditorGUI.Button(text: $"Generate Table Definition Scripts"))
                {
                    try
                    {
                        GenerateScriptAsync(spreadSheetInfo: spreadSheetInfo);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(message: e);
                    }
                    finally
                    {
                        EditorUtility.ClearProgressBar();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            ExEditorGUI.Space();
            ExEditorGUI.Space();
            // ExEditorGUI.LabelFieldAsBold("Table List");
            // if (ExEditorGUI.Button("Reload Table List"))
            // {
            //     SetupTableList();
            // }
            //
            // if (sheetInfoList.IsNullOrEmpty())
            // {
            //     ExEditorGUI.HelpErrorBox("Table List is null or empty.");
            // }
            // else
            // {
            //     scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            //     foreach (SheetInfoEntity sheetInfo in sheetInfoList)
            //     {
            //         EditorGUILayout.BeginHorizontal();
            //         ExEditorGUI.LabelField(sheetInfo.SheetTitle);
            //         if (ExEditorGUI.Button("Update"))
            //         {
            //             // spreadSheetSettingAsset.UpdateMasterData(sheetInfo);
            //         }
            //
            //         EditorGUILayout.EndHorizontal();
            //     }
            //
            //     EditorGUILayout.EndScrollView();
            // }

            EditorGUI.EndDisabledGroup();
            ExEditorGUI.Space();
            ExEditorGUI.Space();

            ExEditorGUI.LabelFieldAsBold(label: "How to Get [Credential.json]");
            if (ExEditorGUI.Button(text: "OPEN Google Site"))
            {
                System.Diagnostics.Process.Start(
                    fileName: @"https://developers.google.com/sheets/api/quickstart/dotnet");
            }

            // https://developers.google.com/drive/api/v3/quickstart/dotnet
            // Credential.json for google drive
        }

        private void SetupSpreadSheetSettingAsset()
        {
            if (ExAssetDatabase.FindAllAssetByTypeName<SpreadSheetSettingAsset>().IsNullOrEmpty())
            {
                const string folderPath = "Assets/App/Editor/Settings/Database";
                SpreadSheetSettingAsset asset =
                    CreateInstance(type: typeof(SpreadSheetSettingAsset)) as SpreadSheetSettingAsset;
                ExIO.CreateDirectoryNotExist(folderPath: folderPath);
                AssetDatabase.CreateAsset(
                    asset: asset,
                    path: Path.Combine(path1: folderPath, path2: nameof(SpreadSheetSettingAsset) + ".asset"));
                AssetDatabase.SaveAssets();
            }

            List<SpreadSheetSettingAsset> assetList = ExAssetDatabase.FindAllAssetByTypeName<SpreadSheetSettingAsset>();
            if (assetList.IsNullOrEmpty())
            {
                Debug.LogError(message: "Not found SpreadSheetSettingAsset.");
                return;
            }

            if (assetList.Count > 1)
            {
                Debug.LogWarning(message: "There are multiple SpreadSheetSettingAsset.");
            }

            spreadSheetSettingAsset = assetList.First();
        }

        private async void SetupTableList()
        {
            sheetInfoList.ClearNotThrow();
            foreach (SpreadSheetInfoEntity spreadSheetInfo in spreadSheetSettingAsset.SpreadSheetInfoList)
            {
                Spreadsheet result = await spreadSheetSettingAsset.GetSheetService().Spreadsheets
                    .Get(spreadsheetId: spreadSheetInfo.SpreadSheetId).ExecuteAsync();
                var sheetList = result.Sheets
                    .Where(predicate: x =>
                        x.Properties.Title.Contains(value: "[T]") || x.Properties.Title.Contains(value: "[E]"))
                    .ToList();
                if (sheetList.IsNullOrEmpty())
                {
                    continue;
                }

                sheetInfoList.AddRange(collection: sheetList.Select(selector: x =>
                    new SheetInfoEntity(sheet: x, spreadSheetId: spreadSheetInfo.SpreadSheetId)));
            }
        }

        private async void GenerateScriptAsync(SpreadSheetInfoEntity spreadSheetInfo)
        {
            Spreadsheet spreadsheet =
                await spreadSheetSettingAsset.GetSheetService().Spreadsheets
                    .Get(spreadsheetId: spreadSheetInfo.SpreadSheetId)
                    .ExecuteAsync();

            var enumSheetList = spreadsheet.Sheets
                .Where(predicate: x => x.Properties.Title.Contains(value: "[E]")).ToList();

            foreach (Sheet sheet in enumSheetList)
            {
                var sheetTitle = sheet.Properties.Title;
                Debug.Log(message: $"sheet title {sheetTitle}");

                ValueRange resultValues = await spreadSheetSettingAsset.GetSheetService().Spreadsheets.Values
                    .Get(spreadsheetId: spreadSheetInfo.SpreadSheetId, range: sheetTitle)
                    .ExecuteAsync();

                var sheetValues = resultValues.Values;
                var logicalName = sheetValues[index: 1][index: 2].ToString();
                var physicalName = sheetValues[index: 2][index: 2].ToString();

                var enumDataEntityList = CreateEnumDataEntityList(sheetValues: sheetValues);
                EnumTableEntity enumTableEntity = new EnumTableEntity(schema: SchemaType.Enumeration.ToString(),
                    enumLogicalName: logicalName,
                    enumPhysicalName: physicalName,
                    enumDataEntityList: enumDataEntityList);
                EnumTableRepository.Me.Add(entity: enumTableEntity);
            }

            foreach (EnumTableEntity entity in EnumTableRepository.Me.FindAll())
            {
                var folderPath = Path.Combine(path1: ScriptAutoGeneratedSaveLocation, path2: "Enumerations");
                var contents = entity.GenerateEnumScript(nameSpace: ScriptNameSpace);
                var enumFilePath = Path.Combine(path1: folderPath, path2: entity.ScriptFileName);
                ExIO.CreateDirectoryNotExist(folderPath: folderPath);
                ExIO.WriteAllTextAsUTF8(filePath: enumFilePath, content: contents);
                RuntimeUnityEditor.AssetDataBaseRefresh();

                string[] labels = {SchemaType.Enumeration.ToString()};
                AssetDatabase.SetLabels(obj: AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath: enumFilePath),
                    labels: labels);
            }

            AssetDatabase.Refresh();

            var tList = spreadsheet.Sheets.Where(predicate: x => x.Properties.Title.Contains(value: "[T]")).ToList();
            foreach (Sheet sheet in tList)
            {
                var sheetTitle = sheet.Properties.Title;
                Debug.Log(message: $"sheet title {sheetTitle}");
                ValueRange resultValues = await spreadSheetSettingAsset.GetSheetService().Spreadsheets.Values
                    .Get(spreadsheetId: spreadSheetInfo.SpreadSheetId, range: sheetTitle)
                    .ExecuteAsync();

                var sheetValues = resultValues.Values;
                var schema = sheetValues[index: 0][index: 2].ToString();
                var logicalName = sheetValues[index: 1][index: 2].ToString();
                var physicalName = sheetValues[index: 2][index: 2].ToString();
                var persistentObjectType = sheetValues[index: 3][index: 2].ToString();

                TableDefinitionEntity entity = new TableDefinitionEntity(
                    schema: schema,
                    logicalName: logicalName,
                    physicalName: physicalName,
                    persistentObjectType: persistentObjectType,
                    data: ToTableDefinitionDataEntityList(sheetValues: sheetValues));

                TableDefinitionRepository.Me.Add(entity: entity);
            }

            foreach (TableDefinitionEntity entity in TableDefinitionRepository.Me.FindAll())
            {
                Debug.Log(message: $"generate {entity.PhysicalName} script");

                // create entity file
                var folderPath = Path.Combine(path1: ScriptAutoGeneratedSaveLocation,
                    path2: entity.SchemaType.ToString());
                var contents = entity.GenerateEntityScript(ScriptNameSpace);
                var filePath = Path.Combine(path1: folderPath, path2: entity.EntityScriptFileName);
                ExIO.CreateDirectoryNotExist(folderPath: folderPath);
                ExIO.WriteAllTextAsUTF8(filePath, contents);

                // create entity service
                var serviceFolderPath = Path.Combine(
                    ScriptSaveLocation,
                    entity.SchemaType.ToString());

                var entityServiceFilePath = Path.Combine(
                    serviceFolderPath,
                    entity.EntityServiceScriptFileName);

                if (!File.Exists(entityServiceFilePath))
                {
                    ExIO.CreateDirectoryNotExist(folderPath: serviceFolderPath);
                    var entityServiceScript = entity.GenerateEntityServiceScript(ScriptNameSpace);
                    ExIO.WriteAllTextAsUTF8(entityServiceFilePath, entityServiceScript);
                }

                var repositoryScript = "";
                var repositoryServiceScript = "";
                var fileName = "";
                var serviceFilePath = "";

                if (entity.PersistentObjectType.IsRepository())
                {
                    repositoryScript = entity.GenerateRepositoryScript(ScriptNameSpace);
                    fileName = entity.RepositoryScriptFileName;

                    repositoryServiceScript = entity.GenerateRepositoryServiceScript(ScriptNameSpace);
                    serviceFilePath = Path.Combine(serviceFolderPath, entity.RepositoryServiceScriptFileName);
                }
                else if (entity.PersistentObjectType.IsDataMapper())
                {
                    repositoryScript = entity.GenerateDataMapperScript(ScriptNameSpace);
                    fileName = entity.DataMapperScriptFileName;

                    repositoryServiceScript = entity.GenerateDataMapperServiceScript(ScriptNameSpace);
                    serviceFilePath = Path.Combine(serviceFolderPath, entity.DataMapperServiceScriptFileName);
                }

                var repositoryFilePath = Path.Combine(folderPath, fileName);
                ExIO.WriteAllTextAsUTF8(repositoryFilePath, repositoryScript);

                if (!File.Exists(serviceFilePath))
                {
                    ExIO.WriteAllTextAsUTF8(serviceFilePath, repositoryServiceScript);
                }

                AssetDatabase.Refresh();

                string[] entityLabels = {entity.SchemaType.ToString(), "Entity", "Database"};
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<TextAsset>(filePath), entityLabels);

                string[] repositoryLabels =
                    {entity.SchemaType.ToString(), entity.PersistentObjectType.ToString(), "Database"};
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<TextAsset>(repositoryFilePath), repositoryLabels);
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        private static List<EnumDataEntity> CreateEnumDataEntityList(IList<IList<object>> sheetValues)
        {
            const int dataStartIndex = 5;

            var result = new List<EnumDataEntity>();
            for (var i = dataStartIndex;
                i < sheetValues.Count;
                i++)
            {
                var row = sheetValues[index: i];
                var index = row.ElementAtOrDefault(index: 0).ToInt(defaultValue: 0);
                var logicalName = row.ElementAtOrDefault(index: 1)?.ToString();
                var physicalName = row.ElementAtOrDefault(index: 2)?.ToString();
                var value = row.ElementAtOrDefault(index: 3).ToInt();
                var description = row.ElementAtOrDefault(index: 4)?.ToString();

                if (index.IsZero() || index.IsNegative())
                {
                    continue;
                }

                EnumDataEntity entity =
                    new EnumDataEntity(index: index, fieldLogicalName: logicalName, fieldPhysicalName: physicalName,
                        value: value, description: description);
                result.Add(item: entity);
            }

            return result;
        }

        private static List<TableDefinitionDataEntity> ToTableDefinitionDataEntityList(
            IList<IList<object>> sheetValues)
        {
            const int dataStartIndex = 5;
            var result = new List<TableDefinitionDataEntity>();
            for (var i = dataStartIndex; i < sheetValues.Count; i++)
            {
                var row = sheetValues[index: i];
                var index = row.ElementAtOrDefault(index: 0).ToInt(defaultValue: 0);
                var fieldLogicalName = row.ElementAtOrDefault(index: 1)?.ToString();
                var fieldPhysicalName = row.ElementAtOrDefault(index: 2)?.ToString();
                var dataType = row.ElementAtOrDefault(index: 3)?.ToString();
                var defaultValue = row.ElementAtOrDefault(index: 6)?.ToString();
                var relation = row.ElementAtOrDefault(index: 7)?.ToString();

                if (index.IsZero() || index.IsNegative())
                {
                    continue;
                }

                // undefined dataType is threw
                if (string.IsNullOrEmpty(value: dataType))
                {
                    continue;
                }

                TableDefinitionDataEntity entity =
                    new TableDefinitionDataEntity(logicalName: fieldLogicalName, physicalName: fieldPhysicalName,
                        dataType: dataType, defaultValue: defaultValue,
                        relation: relation);

                result.Add(item: entity);
            }

            return result;
        }
    }
}
