using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YNQ.Settings.SettingField
{
    [CreateAssetMenu(fileName = "Input Binding Setting Field", menuName = "YNQ /Settings /Fields /Input Binding")]
    public class InputBindingSettingField : AScriptableSettingField<string>
	{
        [field: SerializeField] public InputActionReference InputAction { get; private set; }
        [field: SerializeField] public bool CanRebind { get; private set; }
        
        public override string GetValueDisplay() => Value.ToString();

        protected override string ValidateValue(string value) => value;
    }
}