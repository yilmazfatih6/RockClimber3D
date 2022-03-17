using System.Collections;
using ScriptableObjects.Data;
using ScriptableObjects.Events;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Controls Time.timescale
    /// Listens for onAttachmentComplete event to scale down time scale.
    /// Resets time scale after slowDownDuration
    /// Listens for onAttachmentTargetSelected event to reset time scale.
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private FloatVariable slowDownTimeScale;
        [SerializeField] private FloatVariable slowDownDuration;

        [Header("Listening To")]
        [SerializeField] private VoidEventChannelSO onAttachmentComplete;
        [SerializeField] private GameObjectEventChannelSO onAttachmentTargetSelected;

        #region MonoBehaviour Functions

        private void OnEnable()
        {
            onAttachmentComplete.OnEventRaised += OnAttachedToRock;
            onAttachmentTargetSelected.OnEventRaised += OnAttachmentTargetSelected;
        }
        
        private void OnDisable()
        {
            onAttachmentComplete.OnEventRaised -= OnAttachedToRock;
            onAttachmentTargetSelected.OnEventRaised -= OnAttachmentTargetSelected;
        }
        
        #endregion

        #region Event Responses

        private void OnAttachedToRock()
        {
            StopAllCoroutines();
            StartCoroutine(SlowDownTime());
        }

        private void OnAttachmentTargetSelected(GameObject value)
        {
            StopAllCoroutines();
            UnityEngine.Time.timeScale = 1f;
        }

        #endregion

        #region Private Methods

        private IEnumerator SlowDownTime()
        {
            UnityEngine.Time.timeScale = slowDownTimeScale.Value;

            yield return new WaitForSeconds(slowDownDuration.Value);
        
            UnityEngine.Time.timeScale = 1f;
        }

        #endregion
    }
}