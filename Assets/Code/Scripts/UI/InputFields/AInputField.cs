using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace YNQ.InputField
{
    public abstract class AInputField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected const string BOX_COMPONENTS = "Components";
        protected const string BOX_EVENTS = "Events";
        protected const string BOX_SETTINGS = "Settings";

        [SerializeField, BoxGroup(BOX_SETTINGS)] private string _name;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private Color defaultColor = new (.9f, .9f, .9f, 1);
        [SerializeField, BoxGroup(BOX_SETTINGS)] private Color hoverColor = Color.white;
        [SerializeField, BoxGroup(BOX_COMPONENTS)] protected TextMeshProUGUI nameDisplay;

        private Color _baseColor;

        public event Action OnEnter;
        public event Action OnExit;

        protected virtual void Awake()
        {
            _baseColor = nameDisplay.color;
            ChangeColor(defaultColor);
        }

        public virtual void ResetEvents()
        {

        }

        protected virtual void OnValidate()
        {
            SetName(_name);
        }

        public void SetName(string name)
        {
            if (nameDisplay != null)
                nameDisplay.text = name;
        }

        protected void ChangeColor(Color color)
        {
            nameDisplay.color = _baseColor * color;
        }

        public void OnPointerEnter(PointerEventData eventData) 
        {
            if (InputFieldSelection.current == null)
                Enter();
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            if (InputFieldSelection.current == null)
            {
                Exit();
            }
            else StartCoroutine(WaitForMouseUp());
        }

        private void Enter()
        {
            ChangeColor(hoverColor);

            OnEnter?.Invoke();
        }

        private void Exit()
        {
            ChangeColor(defaultColor);

            OnExit?.Invoke();
        }

        private IEnumerator WaitForMouseUp()
        {
            yield return new WaitUntil(() => InputFieldSelection.current == null);

            Exit();
            var list = new List<RaycastResult>();
            var pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointerData, list);
            foreach(var result in list)
            {
                ExecuteEvents.Execute(
                result.gameObject,
                pointerData,
                ExecuteEvents.pointerEnterHandler
            );
            }
        }
    } 
}
