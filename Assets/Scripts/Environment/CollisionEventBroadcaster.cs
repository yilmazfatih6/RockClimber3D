using ScriptableObjects.Events;
using UnityEngine;

namespace Environment
{
    public class CollisionEventBroadcaster : MonoBehaviour
    {
        #region Properties
        [Header("Properties")]
        [SerializeField] private LayerMask includeLayers;
        [SerializeField] private bool disableCollider;

        [Header("Broadcasting On")]
        [SerializeField] private VoidEventChannelSO eventToBroadcast;
        #endregion

        #region MonoBehaviour Methods

        private void OnCollisionEnter(Collision other)
        {
            if(((1<<other.gameObject.layer) & includeLayers) != 0)
            {
                if (disableCollider) GetComponent<Collider>().enabled = false;
                eventToBroadcast.RaiseEvent();
            }
        }
        #endregion
    }
}