using System;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace QuickUnity.Editor.Database.Entities
{
    [Serializable]
    public class SheetInfoEntity
    {
        [SerializeField] private string sheetTitle;
        [SerializeField] private string spreadSheetId;

        public SheetInfoEntity(Sheet sheet, string spreadSheetId)
        {
            sheetTitle = sheet.Properties.Title;
            this.spreadSheetId = spreadSheetId;
        }

        public string SheetTitle => sheetTitle;
        public string SpreadSheetId => spreadSheetId;
    }
}
