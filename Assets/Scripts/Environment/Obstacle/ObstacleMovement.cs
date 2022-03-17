using DG.Tweening;
using ScriptableObjects.Data;
using UnityEngine;

namespace Environment.Obstacle
{
    /// <summary>
    /// Moves attached object with given properties.
    /// </summary>
    public class ObstacleMovement : MonoBehaviour
    {
        #region Properties

        [Header("Properties")] 
        [SerializeField] private Transform movementTarget;
        [SerializeField] private FloatVariable speed;
        [SerializeField] private FloatVariable maxDelay;
        [SerializeField] private Ease ease;
        
        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
            Move();
        }

        #endregion

        #region Private Methods

        private void Move()
        {
            float delay = Random.Range(0, maxDelay.Value);
            transform.DOLocalMove(movementTarget.localPosition, 1 / speed.Value)
                .SetEase(ease)
                .SetUpdate(UpdateType.Fixed)
                .SetLoops(2, LoopType.Yoyo)
                .SetDelay(delay)
                .OnComplete(Move);
        }        

        #endregion
    }
}