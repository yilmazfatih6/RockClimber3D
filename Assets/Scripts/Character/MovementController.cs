using System.Collections;
using DG.Tweening;
using ScriptableObjects.Data;
using ScriptableObjects.Events;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Controls ragdoll character's movement.
    /// Listens to onAttachmentTargetSelected event. Then performs movement to target.
    /// Listens to onObstacleHit event to cancel movement.
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        #region Properties

        [Header("Listening To")] 
        [SerializeField] private GameObjectEventChannelSO onAttachmentTargetSelected;
        [SerializeField] private VoidEventChannelSO onObstacleHit;
    
        [Header("Broadcasting On")] 
        [SerializeField] private VoidEventChannelSO onAttachmentComplete;
    
        [Header("Joints")]
        [SerializeField] private GameObject leftHandJoint;
        [SerializeField] private GameObject rightHandJoint;

        [Header("Joint Targets")] 
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
    
        [Header("Properties")] 
        [SerializeField] private FloatVariable speed;
        [SerializeField] private FloatVariable movementDelay;
        [SerializeField] private Ease ease;
    
        private Vector3 leftHandAttachmentRotation = new Vector3(0,90,-90);
        private Vector3 rightHandAttachmentRotation =  new Vector3(0,90,90);
        private bool isLeftHandAttached;
        private Tweener movementTween;
        private Tweener rotationTween;
        private bool isFirstMovement;

        #endregion

        #region MonoBehaviour Functions

        private void OnEnable()
        {
            onAttachmentTargetSelected.OnEventRaised += OnAttachmentTargetSelected;
            onObstacleHit.OnEventRaised += DisableMovement;
        }

        private void OnDisable()
        {
            onAttachmentTargetSelected.OnEventRaised -= OnAttachmentTargetSelected;
            onObstacleHit.OnEventRaised -= DisableMovement;
            isFirstMovement = true;
        }

        #endregion
    
        #region Event Responses

        private void OnAttachmentTargetSelected(GameObject value)
        {
            if (movementTween != null && movementTween.active) return;
            
            Debug.Log("[JointsController.cs] OnAttachmentTargetSelected");

            if (isLeftHandAttached)
            {
                // Attach right hand
                isLeftHandAttached = false;
                StartCoroutine(
                    SetUpNewAttachment(
                        leftHandJoint, 
                        rightHandJoint, 
                        value, 
                        rightHand, 
                        rightHandAttachmentRotation)
                );
            }
            else
            {
                // Attach left hand
                isLeftHandAttached = true;
                StartCoroutine(
                    SetUpNewAttachment(
                        rightHandJoint, 
                        leftHandJoint, 
                        value, 
                        leftHand, 
                        leftHandAttachmentRotation)
                );
            }
        }
    
        private void DisableMovement()
        {
            rotationTween?.Kill();
            movementTween?.Kill();
        
            leftHandJoint.GetComponent<FixedJoint>().connectedBody = null;
            rightHandJoint.GetComponent<FixedJoint>().connectedBody = null;
        }

        #endregion

        #region Private Methods

        private void MoveJointToTarget(GameObject jointToMove, GameObject target, Vector3 attachmentRotation)
        {
            float distance = Vector3.Distance(target.transform.position, jointToMove.transform.position);
            float duration = distance / speed.Value;
        
            movementTween = jointToMove.transform.DOMove(target.transform.position, duration)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(ease)
                .OnComplete(() => onAttachmentComplete.RaiseEvent());

            rotationTween = jointToMove.transform.DORotate(attachmentRotation, duration)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(ease);
        }

        private IEnumerator SetUpNewAttachment(GameObject jointToRemove, GameObject jointToAttach, GameObject movementTarget, GameObject targetHand, Vector3 attachmentRotation)
        {
            // Clear left hand
            jointToRemove.GetComponent<FixedJoint>().connectedBody = null;

            if (isFirstMovement)
            {
                isFirstMovement = false;
                yield return new WaitForSeconds(0);
            }
            else
            {
                yield return new WaitForSeconds(movementDelay.Value);
            }

            // Attach right hand
            jointToAttach.transform.rotation = targetHand.transform.rotation;
            jointToAttach.transform.position = targetHand.transform.position;
            jointToAttach.GetComponent<FixedJoint>().connectedBody = targetHand.GetComponent<Rigidbody>();
            
            MoveJointToTarget(jointToAttach, movementTarget, attachmentRotation);
        }

        #endregion
    }
}

