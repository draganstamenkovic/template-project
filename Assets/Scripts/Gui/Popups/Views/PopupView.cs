using System;
using Configs;
using DG.Tweening;
using Gui.Popups.Builder;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Gui.Popups.Views
{
    public abstract class PopupView : MonoBehaviour, IPopupView
    {
        [Inject] private PopupsConfig _popupsConfig;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _backgroundButton;
        public Button BackgroundButton => _backgroundButton;
        public string ID { get; set; }
        public Action OnShow { get; set; }
        public Action OnShown { get; set; }
        public Action OnHide { get; set; }
        public Action OnHidden { get; set; }

        public virtual void Initialize()
        {
            _canvasGroup.alpha = 0;
        }

        public virtual void Show(Action onComplete = null)
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1f, _popupsConfig.ShowTransitionDuration)
                .SetEase(_popupsConfig.TransitionEase).OnComplete(() =>
                {
                    OnShown?.Invoke();
                    onComplete?.Invoke();
                });
        }

        public virtual void Hide(Action onComplete = null)
        {
            OnHide?.Invoke();
            _canvasGroup.DOFade(0f, _popupsConfig.HideTransitionDuration)
                .SetEase(_popupsConfig.TransitionEase).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnHidden?.Invoke();
                    onComplete?.Invoke();
                });
        }
    }
}