using System;
using UnityEngine;

namespace Utilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]

    public class AlwaysExpanded : PropertyAttribute
    {
        public bool HideLabel { get; }

        public AlwaysExpanded(bool hideLabel)
        {
            HideLabel = hideLabel;
        }

        public AlwaysExpanded()
        {
            HideLabel = false;
        }
    }
}