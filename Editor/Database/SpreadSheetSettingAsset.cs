using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using QuickUnity.Editor.Database.Entities;
using UnityEditor;
using UnityEngine;

namespace QuickUnity.Editor.Database
{
    public class SpreadSheetSettingAsset : ScriptableObject
    {
        [SerializeField] private TextAsset spreadSheetCredentialJson;

        public List<SpreadSheetInfoEntity> SpreadSheetInfoList = new List<SpreadSheetInfoEntity>();

        public SheetsService GetSheetService()
        {
            if (spreadSheetCredentialJson == null)
            {
                throw new ArgumentNullException($"credetial.json is null.");
            }

            var path = AssetDatabase.GetAssetPath(spreadSheetCredentialJson);
            UserCredential credential;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                const string tokenFileName = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets: GoogleClientSecrets.Load(stream).Secrets,
                    new List<string> {SheetsService.Scope.SpreadsheetsReadonly},
                    "user", CancellationToken.None,
                    dataStore: new FileDataStore(tokenFileName, true)).Result;
                Debug.Log("Credential file saved to: " + tokenFileName);
            }

            // Create Google Sheets API service.
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential, ApplicationName = "QuickUnity"
            });
        }

        // public void UpdateMasterData(SheetInfo sheetInfo)
        // {
        //     EditorUtility.DisplayProgressBar(sheetInfo.Title, "", 0);
        //     ValueRange result = GetSheetService().Spreadsheets.Values.Get(sheetInfo.SpreadSheetId, sheetInfo.Title)
        //         .Execute();
        //     var sheetValues = result.Values;
        //
        //     var repositoryName = "Master" + sheetInfo.Title.Replace("[D]", "").ConvertsSnakeToUpperCamel() +
        //                          "Repository";
        //
        //     Debug.Log($"repository name : {repositoryName}");
        //
        //     Type repositoryType = repositoryName.ToType();
        //
        //     Debug.Log($"repository type : {repositoryType.Name}");
        //
        //     IList<string> fieldKeys = sheetValues[1].Select(x => x.ToString()).ToList();
        //     var values = new List<Dictionary<string, object>>();
        //
        //     for (var index = 3; index < sheetValues.Count; index++)
        //     {
        //         var row = sheetValues[index];
        //         var dictionary = new Dictionary<string, object>();
        //         for (var i = 0; i < fieldKeys.Count; i++)
        //         {
        //             var key = fieldKeys[i];
        //             object value = row.ElementAtOrDefault(i);
        //             if (string.IsNullOrEmpty(key) || value == null)
        //             {
        //                 continue;
        //             }
        //
        //             dictionary.Add(key, value);
        //         }
        //
        //         values.Add(dictionary);
        //     }
        //
        //     try
        //     {
        //         object repositoryInstance = Activator.CreateInstance(repositoryType);
        //         DatabaseReflection.InsertAll(repositoryType, repositoryInstance, values);
        //
        //         var json = JsonUtility.ToJson(repositoryInstance, true);
        //         IDatabase database = repositoryInstance as IDatabase;
        //         File.WriteAllText(
        //             Path.Combine(Application.dataPath, "../", DatabaseSettings.Location.FolderName, database.Schema,
        //                 database.KName + ".json"), json);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError(e.ToString());
        //         throw;
        //     }
        //     finally
        //     {
        //         RuntimeUnityEditor.AssetDataBaseRefresh();
        //         EditorUtility.ClearProgressBar();
        //     }
        // }
        //
        // public void UpdateAllMasterData(List<SheetInfo> sheetInfoList)
        // {
        //     foreach (SheetInfo sheetInfo in sheetInfoList)
        //     {
        //         UpdateMasterData(sheetInfo);
        //     }
        // }
    }
}
