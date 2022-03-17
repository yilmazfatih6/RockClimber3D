using DG.Tweening;
using ScriptableObjects.Data;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeSlowDownUI : MonoBehaviour
    {
        #region Properties

        [Header("Properties")]
        [SerializeField] private FloatVariable slowDownDuration;
        [SerializeField] private Slider slider;

        [Header("Listening To")]
        [SerializeField] private VoidEventChannelSO onAttachmentComplete;
        [SerializeField] private GameObjectEventChannelSO onAttachmentTargetSelected;
        [SerializeField] private VoidEventChannelSO onLevelComplete;

        private Tween tween;
        
        #endregion
        
        #region MonoBehaviour Functions

        private void Start()
        {
            DisableTimer();
        }

        private void OnEnable()
        {
            onAttachmentComplete.OnEventRaised += OnAttachedToRock;
            onAttachmentTargetSelected.OnEventRaised += OnAttachmentTargetSelected;
            onLevelComplete.OnEventRaised += DisableUI;
        }
        
        private void OnDisable()
        {
            onAttachmentComplete.OnEventRaised -= OnAttachedToRock;
            onAttachmentTargetSelected.OnEventRaised -= OnAttachmentTargetSelected;
            onLevelComplete.OnEventRaised -= DisableUI;
        }
        
        #endregion

        #region Event Responses

        private void OnAttachedToRock()
        {
            tween?.Kill();
            AnimateSlider();
        }

        private void OnAttachmentTargetSelected(GameObject value)
        {
            tween?.Kill();
            DisableTimer();
        }

        #endregion

        #region Private Methods

        private void AnimateSlider()
        {
            DOVirtual.Float(1, 0, slowDownDuration.Value, (value) =>
            {
                slider.value = value;
            })
                .SetEase(Ease.Linear)
                .OnComplete(DisableTimer);
        }

        private void DisableTimer()
        {
            slider.value = 0;
        }

        private void DisableUI()
        {
            slider.GetComponentInChildren<Image>().enabled = false;
        }
        #endregion
    }
}