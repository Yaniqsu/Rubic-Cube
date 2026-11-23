#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Utilities
{
    [CustomPropertyDrawer(typeof(AlwaysExpanded), true)]

    public class AlwaysExpandedDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var alwaysExpandedAttribute = attribute as AlwaysExpanded;

            var hideLabel = alwaysExpandedAttribute!.HideLabel;
            
            EditorGUI.BeginProperty(position, label, property);

            if(!hideLabel)
            {
                EditorGUI.LabelField(
                    new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), 
                    label,
                    new GUIStyle {fontStyle = FontStyle.Normal, normal = { textColor = Color.white }});
            }
            
            if(!hideLabel)
                EditorGUI.indentLevel++;

            var child = property.Copy();
            var endProperty = child.GetEndProperty();
            
            if(!hideLabel)
                position.y += EditorGUIUtility.singleLineHeight;

            while (child.NextVisible(true) && !SerializedProperty.EqualContents(child, endProperty))
            {
                position.height = EditorGUI.GetPropertyHeight(child);
                EditorGUI.PropertyField(position, child, true);
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            }

            if(!hideLabel)
                EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var alwaysExpandedAttribute = attribute as AlwaysExpanded;
            
            var hideLabel = alwaysExpandedAttribute!.HideLabel;
            
            var height = hideLabel ? 0f : EditorGUIUtility.singleLineHeight;

            var child = property.Copy();
            var endProperty = child.GetEndProperty();

            while (child.NextVisible(true) && !SerializedProperty.EqualContents(child, endProperty))
            {
                height += EditorGUI.GetPropertyHeight(child) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}
#endif
