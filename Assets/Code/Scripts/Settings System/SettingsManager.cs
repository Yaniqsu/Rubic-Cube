using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using YNQ.Settings.SaveSystem;
using YNQ.Settings.UI;

namespace YNQ.Settings
{
	public class SettingsManager : MonoBehaviour, IMenuComponent
	{
		[SerializeField] private SettingsDatabase database;
		[SerializeField] private SettingsMenuManager menuManager;
		[SerializeField] private SettingsMenuAnimator animator;
		[SerializeField, BoxGroup("Dialog Box")] private DialogBox dialogBox;
		[SerializeField, BoxGroup("Dialog Box")] private DialogContent applyChangesContent;
		[SerializeField, BoxGroup("Dialog Box")] private DialogContent changesAppliedContent;

		public UnityEvent onBackStarted;
		public UnityEvent onBackFinished;
		public event Action onShowAnimationEnd;

        private void Start()
        {
            menuManager.Initialize(database);

			menuManager.onApply.AddListener(ApplyChanges);
			menuManager.onBack.AddListener(Back);
			menuManager.onShowAnimationEnd.AddListener(() => onShowAnimationEnd?.Invoke());

			animator.onBackAnimationEnd.AddListener(onBackFinished.Invoke);

            if (SettingsSaveManager.CanLoad())
            {
                database.LoadSave(SettingsSaveManager.Load());
            }
        }

        public void Show() 
		{
			menuManager.Show();
		}

		private void Hide()
		{
			menuManager.Hide();
			onBackStarted.Invoke();
		}

		private void ApplyChanges()
		{
			if (!database.IsDirty())
				return;
			
			dialogBox.Show(changesAppliedContent);

			dialogBox.OnPositiveButtonClicked += () => {
				database.ApplyChanges();
				SettingsSaveManager.Save(database.GetSave());
			};
        }

		private void Back()
		{
			if (animator.Animating)
				return;

            if (database.IsDirty())
			{
				dialogBox.Show(applyChangesContent);
                dialogBox.OnPositiveButtonClicked += () =>
                {
                    database.ApplyChanges();
					Hide();
                };
                dialogBox.OnNegativeButtonClicked += () =>
                {
                    database.RevertTemporaryChanges();
					Hide();
                };
            }
			else
			{
				Hide();
			}

            SettingsSaveManager.Save(database.GetSave());
        }

        void IMenuComponent.Back() => Back();
		public bool CanGoBack() => !animator.Animating;
    } 
}
