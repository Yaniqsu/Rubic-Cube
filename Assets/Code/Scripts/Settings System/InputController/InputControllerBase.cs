using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
    public abstract class InputControllerBase : MonoBehaviour
    {
        public abstract void Connect(ScriptableSettingFieldBase settingField);
        public abstract void Disconnect(ScriptableSettingFieldBase settingField);

        public event Action OnEnter;
        public event Action OnExit;

        public abstract Type GetTypeAssociated();

        protected void CallOnEnter()
        {
            OnEnter?.Invoke();
        }

        protected void CallOnExit()
        {
            OnExit?.Invoke();
        }
    }
}