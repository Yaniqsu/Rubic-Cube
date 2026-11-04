using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YNQ.Settings.UI
{
	public class SettingsMenuManager : MonoBehaviour
	{
        [BoxGroup("Controllers"), SerializeField] private SettingsGroupsController groupsController;
		[BoxGroup("Controllers"), SerializeField] private SettingFieldsController settingFieldsController;
		[BoxGroup("Controllers"), SerializeField] private SettingsMenuAnimator menuAnimator;

        [BoxGroup("Components"), SerializeField] private TextButton backButton;
        [BoxGroup("Components"), SerializeField] private TextButton applyButton;
        [BoxGroup("Components"), SerializeField] private CanvasGroup loadingCanvasGroup;
        [BoxGroup("Components"), SerializeField] private CanvasGroup settingsCanvasGroup;

        public UnityEvent onApply;
        public UnityEvent onBack;
        public UnityEvent onShowAnimationEnd;

        public void Initialize(SettingsDatabase database)
        {
            groupsController.Initialize(database.Groups);

            groupsController.onGroupChanged.AddListener(
                groupIndex => settingFieldsController.ChangeGroup(database.Groups[groupIndex]));

            menuAnimator.onShowAnimationEnd.AddListener(onShowAnimationEnd.Invoke);

            database.ResetGroups();

            applyButton.onClick.AddListener(Apply);
            backButton.onClick.AddListener(Back);
        }

        public void Show()
        {
            menuAnimator.PlayShowAnimation();
        }

        public void Hide()
        {
            menuAnimator.PlayHideAnimation();
            groupsController.DeselectAll();
            settingFieldsController.ClearInputControllers();
        }

        private void Apply() => onApply.Invoke();

        private void Back() => onBack.Invoke();
    } 
}
