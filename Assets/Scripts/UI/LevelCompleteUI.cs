using System.Collections;
using ScriptableObjects.Data;
using ScriptableObjects.Events;
using UnityEngine;

namespace UI
{
    public class LevelCompleteUI : MonoBehaviour
    {
        #region Properties

        [Header("Properties")] 
        [SerializeField] private Canvas levelCompleteCanvas;
        [SerializeField] private FloatVariable levelCompleteDisplayDelay;
            
        [Header("Listening To")]
        [SerializeField] private VoidEventChannelSO onLevelComplete;

        #endregion
        
        #region MonoBehaviour Methods

        private void Awake()
        {
            levelCompleteCanvas.enabled = false;
        }

        private void OnEnable()
        {
            onLevelComplete.OnEventRaised += OnLevelComplete;
        }
        
        private void OnDisable()
        {
            onLevelComplete.OnEventRaised -= OnLevelComplete;
        }

        #endregion
        
        #region Event Responses

        private void OnLevelComplete()
        {
            StartCoroutine(DisplayLevelCompleteUI());
        }
        
        #endregion

        #region Private Methods

        private IEnumerator DisplayLevelCompleteUI()
        {
            yield return new WaitForSeconds(levelCompleteDisplayDelay.Value);
            levelCompleteCanvas.enabled = true;
        }

        #endregion
    }
}