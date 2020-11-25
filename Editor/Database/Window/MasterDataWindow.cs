using Editor.Extensions;
using QuickUnity.Extensions.DotNet;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace IMDB4Unity.Editor
{
    internal class MasterDataWindow : EditorWindow
    {
        private static readonly MasterDataWindow Window = null;
        private SpreadSheetSettingAsset spreadSheetSettingAsset;
        private Vector2 scrollPos = Vector2.zero;
        private List<SheetInfo> sheetInfoList;

        [MenuItem("Tools/QuickUnity/Database/MasterDataEditor")]
        static void ShowWindow()
        {
            if (Window != null)
            {
                Window.Close();
            }

            GetWindow<MasterDataWindow>(nameof(MasterDataWindow)).Show();
        }

        private void OnEnable()
        {
            if (ExAssetDatabase.FindAllAssetByTypeName<SpreadSheetSettingAsset>().IsNullOrEmpty())
            {
                SpreadSheetSettingAsset asset =
                    CreateInstance(typeof(SpreadSheetSettingAsset)) as SpreadSheetSettingAsset;
                ExIO.CreateDirectoryNotExist("Assets/App/Editor/");
                AssetDatabase.CreateAsset(asset,
                    Path.Combine("Assets/App/Editor/", nameof(SpreadSheetSettingAsset) + ".asset"));
                AssetDatabase.SaveAssets();
            }

            setup();
        }

        private void setup()
        {
            List<SpreadSheetSettingAsset> assetList = ExAssetDatabase.FindAllAssetByTypeName<SpreadSheetSettingAsset>();
            if (assetList.IsNullOrEmpty())
            {
                Debug.LogError("not found SpreadSheetSettingAsset.");
                return;
            }

            spreadSheetSettingAsset = assetList.First();
        }

        private void OnGUI()
        {
            if (ExEditorGUI.ButtonAsToolBar("Reload SpreadSheets"))
            {
                GetMasterDataSheets();
            }

            if (sheetInfoList.IsNullOrEmpty())
            {
                ExEditorGUI.HelpErrorBox("Master Data Sheet List is null or empty.");
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (SheetInfo sheetInfo in sheetInfoList)
            {
                EditorGUILayout.BeginHorizontal();
                ExEditorGUI.LabelField(sheetInfo.Title);
                if (ExEditorGUI.Button("Update"))
                {
                    spreadSheetSettingAsset.UpdateMasterData(sheetInfo);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (ExEditorGUI.Button("All Update"))
            {
                spreadSheetSettingAsset.UpdateAllMasterData(sheetInfoList);
            }
        }

        private async void GetMasterDataSheets()
        {
            List<SheetInfo> list = new List<SheetInfo>();
            foreach (SpreadSheetInfo spreadSheetInfo in spreadSheetSettingAsset.SpreadSheetInfoList)
            {
                var result = await spreadSheetSettingAsset.GetSheetService().Spreadsheets
                    .Get(spreadSheetInfo.SpreadSheetId).ExecuteAsync();
                var l = result.Sheets.Where(x => x.Properties.Title.Contains("[D]")).ToList();
                if (l.IsNullOrEmpty())
                {
                    continue;
                }

                list.AddRange(l.Select(x => new SheetInfo(x, spreadSheetInfo.SpreadSheetId)));
            }

            sheetInfoList = list;
        }
    }
}
#endif