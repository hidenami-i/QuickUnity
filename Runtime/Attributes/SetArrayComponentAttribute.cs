using System;

namespace QuickUnity.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SetArrayComponentAttribute : Attribute
    {
        public string MatchName { get; }

        public SetArrayComponentAttribute(string matchTargetGameObjectName)
        {
            MatchName = matchTargetGameObjectName;
        }
    }
}