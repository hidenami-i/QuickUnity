using System;
using UnityEngine;

namespace QuickUnity.Editor.Database.Entities
{
    [Serializable]
    public class SpreadSheetInfoEntity
    {
        [SerializeField] private string name = "";
        [SerializeField] private string spreadSheetId = "";

        public string Name => name;
        public string SpreadSheetId => spreadSheetId;
    }
}