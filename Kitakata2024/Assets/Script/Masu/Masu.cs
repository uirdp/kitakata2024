using System;
using Cysharp.Threading.Tasks;
using SakeShooterSystems;
using UnityEngine;

namespace SakeShooter
{
    public class Masu : MonoBehaviour
    {
        [Tooltip("升が消えるまでの初期位置からの距離")]
        public float DistanceThreshold = 100.0f;
        
        // ここらへんのはgetcomponetする変わりに持たせているだけ
        [SerializeField] private MasuStatus _status;
        [SerializeField] private MasuMovement _movement;
        [SerializeField] private BoxHitArea _collider;
        [SerializeField] private MasuResult _result;
        
        private Vector3 _initialPosition;
        public MasuStatus Status => _status;
        public MasuMovement Movement => _movement;
        public MasuResult Result => _result;
        public BoxHitArea Collider => _collider;
        
        private async UniTaskVoid CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, _initialPosition);
            if (distance > DistanceThreshold)
            {
                await _result.RaiseFailureEvent();
            }
        }

        private void Update()
        {
            CheckDistanceAndReturnToPool();   
        }
    }
}