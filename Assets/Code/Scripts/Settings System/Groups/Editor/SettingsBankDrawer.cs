#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using YNQ.Settings.SettingField;
using YNQ.Utilities;

namespace YNQ.Settings.Bank.Editor
{
    public class SettingsBankDrawer : UnityEditor.Editor
	{
        private SerializedProperty m_settingFields;
        private Type[] m_settingFieldTypes;
        private string[] m_settingFieldNames;

        private void OnEnable()
        {
            m_settingFields = serializedObject.FindProperty("settingFields");

            m_settingFieldTypes = ReflectionUtilities.GetInheritors(typeof(ScriptableSettingFieldBase));
            m_settingFieldNames = m_settingFieldTypes.Select(t => t.Name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Popup(0, m_settingFieldNames);
        }
	}
}

#endif