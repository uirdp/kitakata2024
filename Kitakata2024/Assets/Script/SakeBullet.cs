using System;
using UnityEngine;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        [Header("------ Parameters ------")]
        public float speed = 4.5f;
        public float gravity = 0.8f;
        public float distanceThreshold = 10.0f;

        private Vector3 _direction;
        private Vector3 _initialPosition;
        private Action<SakeBullet> _outOfRangeAction;
        

        private void Update()
        {
            Move();
            CheckDistanceAndReturnToPool();
        }

        private void Move()
        {
            // Gravity
            // _direction.y -= gravity * Time.deltaTime;
            this.transform.position += _direction * speed * Time.deltaTime;
        }

        private void CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, _initialPosition);
            if (distance > distanceThreshold)
            {
               _outOfRangeAction?.Invoke(this);
            }
        }
        
        public void RegisterOutOfScopeAction(Action<SakeBullet> action)
        {
            _outOfRangeAction += action;
        }
        
        public void Initialize(Vector3 position, Vector3 direction)
        {
            this.transform.position = position;
            this.transform.forward = direction;
            _initialPosition = position;
            _direction = direction;
        }
    }
}