using ScriptableObjects.Events;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Checks for mouse input to detect if any rock is hit.
    /// Broadcasts onAttachmentTargetSelected if ray hit's any object with includeLayers.
    /// Listens for onLevelComplete to disable input reading.
    /// </summary>
    public class ObjectSelector : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private LayerMask includeLayers;
    
        [Header("Broadcasting On")] 
        [SerializeField] private GameObjectEventChannelSO onAttachmentTargetSelected;
        
        [Header("Listening To")] 
        [SerializeField] private VoidEventChannelSO onLevelComplete;

        private bool isActive = true;

        private void OnEnable()
        {
            onLevelComplete.OnEventRaised += Disable;
        }

        private void OnDisable()
        {
            onLevelComplete.OnEventRaised -= Disable;
        }

        private void Update()
        {
            if (!isActive) return;

            if (!Input.GetMouseButtonDown(0)) return;
            
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, includeLayers);
            if (hit) 
            {
                onAttachmentTargetSelected.RaiseEvent(hitInfo.collider.gameObject);
            }
        }

        private void Disable()
        {
            isActive = false;
        }
    }
}