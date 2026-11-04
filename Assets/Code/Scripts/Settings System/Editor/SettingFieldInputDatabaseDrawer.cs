#if UNITY_EDITOR

using UnityEditor;
using YNQ.Settings.InputController;
using YNQ.Utilities;

namespace YNQ.Settings.Editor
{
	[CustomEditor(typeof(SettingFieldInputDatabase))]
	public class SettingFieldInputDatabaseDrawer : UnityEditor.Editor
	{
		private SerializedProperty m_inputControllers;
        private SDictionary<string, InputControllerBase> m_dictionary;

        private void OnEnable()
        {
            m_inputControllers = serializedObject.FindProperty("inputControllers");
            m_dictionary = m_inputControllers.boxedValue as SDictionary<string, InputControllerBase>;

            CreateControllersList();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            foreach(var row in m_dictionary)
            {
                var type = System.Type.GetType(row.key);
                var name = ObjectNames.NicifyVariableName(type.Name);
                var reference = EditorGUILayout.ObjectField(name, row.value, type, false);

                m_dictionary[row.key] = reference as InputControllerBase;
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this);
                m_inputControllers.boxedValue = m_dictionary;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void CreateControllersList()
        {
            var types = ReflectionUtilities.GetInheritors(typeof(InputControllerBase));

            foreach(var t in types)
            {
                m_dictionary.TryAdd(t.AssemblyQualifiedName, null);
            }

            m_inputControllers.boxedValue = m_dictionary;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }
    }
} 
#endif
