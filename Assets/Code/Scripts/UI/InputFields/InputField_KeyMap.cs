using System;
using System.Threading.Tasks;
using NaughtyAttributes;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace YNQ.InputField
{
    public class InputField_KeyMap : AInputField
    {
        [SerializeField, BoxGroup(BOX_SETTINGS)] private InputAction _action;
        [SerializeField, BoxGroup(BOX_COMPONENTS)] private TextButton _rebindButton;
        [BoxGroup(BOX_EVENTS)] public UnityEvent<InputAction> OnRebind;

        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

        public InputAction Action => _action;

        public void Initialize(InputAction action, bool canBeRemaped)
        {
            nameDisplay.text = action.name;
            _action = action;

            UpdateControl();
        }

        public void UpdateControl()
            => _rebindButton.Text = _action.bindings.Count > 0 ? $"[{_action.bindings[0].ToDisplayString()}]" : "No Binding";

        public void Rebind()
        {
            _rebindButton.Text = "Rebinding...";
            PerformRebind();
        }

        private async void PerformRebind()
        {
            await Task.Delay(10);

            var action = Action;

            if (_rebindOperation != null)
            {
                _rebindOperation.Cancel();
                _rebindOperation.Dispose();
            }

            action.Disable();

            _rebindOperation = action.PerformInteractiveRebinding()
                .WithControlsExcluding("<Mouse>/position")
                .WithControlsExcluding("<Mouse>/delta")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation =>
                {
                    action.Enable();
                    operation.Dispose();

                    UpdateControl();
                    OnRebind.Invoke(action);
                })
                .OnCancel(operation =>
                {
                    action.Enable();
                    operation.Dispose();
                    UpdateControl();
                });

            _rebindOperation.Start();
        }
    } 
}
