using System;
using System.IO;
using QuickUnity.Extensions.Unity;

namespace QuickUnity.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SetObjectFromAssetsAttribute : Attribute
    {
        public string AssetName { get; }
        public Type SearchFilterType { get; }

        public string AssetNameWithoutExtension => Path.GetFileNameWithoutExtension(AssetName);

        public bool IsPrefab => Path.GetExtension(AssetName) == ".prefab";

        public SetObjectFromAssetsAttribute(string assetNameWithExtension)
        {
            if (string.IsNullOrEmpty(assetNameWithExtension))
            {
                ExDebug.LogError("Asset name is null or empty.");
                return;
            }

            AssetName = assetNameWithExtension;
        }

        public SetObjectFromAssetsAttribute(string assetNameWithExtension, Type searchFilterType)
        {
            if (string.IsNullOrEmpty(assetNameWithExtension))
            {
                ExDebug.LogError("Asset name is null or empty.");
                return;
            }

            if (searchFilterType == null)
            {
                ExDebug.LogError("Type is null.");
                return;
            }

            SearchFilterType = searchFilterType;
            AssetName = assetNameWithExtension;
        }
    }
}