using System;
using UnityEngine;

namespace SakeShooter
{
    public class Masu : MonoBehaviour
    {
        [Tooltip("升が消えるまでの初期位置からの距離")]
        public float DistanceThreshold = 100.0f;
        
        [SerializeField] private MasuStatus _status;
        [SerializeField] private MasuMovement _movement;
        private event Action<Masu> _outOfRangeAction;
        private Vector3 _initialPosition;
        
        
        public MasuStatus Status => _status;
        public MasuMovement Movement => _movement;
        
        public void RegisterOutOfRangeAction(Action<Masu> action)
        {
            _outOfRangeAction += action;
        }
        
        private void UnRegisterOutOfRangeAction()
        {
            _outOfRangeAction = null;
        }
        
        private void OnDestroy()
        {
            UnRegisterOutOfRangeAction();
        }
        
        private void CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, _initialPosition);
            if (distance > DistanceThreshold)
            {
                _outOfRangeAction?.Invoke(this);
            }
        }
    }
}