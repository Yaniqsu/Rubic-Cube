using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using NUnit.Framework;
using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace YNQ.InputField
{
    public class InputField_KeyMap : AInputField
    {
        private struct BindingReference
        {
            public TextButton button;
            public int bindingIndex;
        }
        
        [SerializeField, BoxGroup(BOX_SETTINGS)] private InputAction _action;
        [SerializeField, BoxGroup(BOX_COMPONENTS)] private RectTransform _buttonsContext;
        [SerializeField, BoxGroup(BOX_COMPONENTS)] private TextButton rebindButtonPrefab;
        [BoxGroup(BOX_EVENTS)] public UnityEvent<InputAction> OnRebind;

        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

        private List<BindingReference> _references = new();

        public InputAction Action => _action;

        public void Initialize(InputAction action, bool canBeRemaped)
        {
            nameDisplay.text = action.name;
            _action = action;

            var buttonsCount = 0;
            for(int i = 0; i < _action.bindings.Count; i++)
            {
                if (_action.bindings[i].isComposite)
                    continue;

                var index = buttonsCount;
                var button = Instantiate(rebindButtonPrefab, _buttonsContext);
                button.onClick.AddListener(() => Rebind(index));

                _references.Add(new BindingReference() { button = button, bindingIndex = i });

                UpdateControlDisplay(buttonsCount);
                buttonsCount++;
            }
        }

        public void UpdateControlDisplay(int referenceIndex)
        {
            if (referenceIndex < 0 || referenceIndex >= _action.bindings.Count)
                return;

            var bindingIndex = _references[referenceIndex].bindingIndex;
            _references[referenceIndex].button.Text = $"[{_action.bindings[bindingIndex].ToDisplayString()}]";
        }

        public void Rebind(int referenceIndex)
        {
            _references[referenceIndex].button.Text = "Rebinding...";
            PerformRebind(referenceIndex);
        }

        private async void PerformRebind(int referenceIndex)
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
                .WithTargetBinding(_references[referenceIndex].bindingIndex)
                .OnComplete(operation =>
                {
                    action.Enable();
                    operation.Dispose();

                    UpdateControlDisplay(referenceIndex);
                    OnRebind.Invoke(action);
                })
                .OnCancel(operation =>
                {
                    action.Enable();
                    operation.Dispose();
                    UpdateControlDisplay(referenceIndex);
                });

            _rebindOperation.Start();
        }
    } 
}
