using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    /// <summary>
    /// This class is used for Events that have no arguments (Example: Exit game event)
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : DescriptionBaseSO
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}