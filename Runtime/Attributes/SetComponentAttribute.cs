using System;

namespace QuickUnity.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SetComponentAttribute : Attribute
    {
        public string TargetGameObjectName { get; }

        /// <summary>
        ///　If you specify the name of the target object, set the inspector to search for it based on its name.
        /// </summary>
        public SetComponentAttribute()
        {
        }

        /// <summary>
        /// If you specify the name of the target object, set the inspector to search for it based on its name.
        /// </summary>
        /// <param name="targetGameObjectName"></param>
        public SetComponentAttribute(string targetGameObjectName)
        {
            TargetGameObjectName = targetGameObjectName;
        }
    }
}
