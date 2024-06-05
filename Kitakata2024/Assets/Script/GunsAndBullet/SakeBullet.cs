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
        private float _verticalSpeed = 0.0f; 
        private Action<SakeBullet> _outOfRangeAction;
        

        private void Update()
        {
            Move();
            CheckDistanceAndReturnToPool();
        }

        private void Move()
        {
            // This game does not require realistic simulation of gravity and other physics
            _direction.y -= gravity * Time.deltaTime;
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
        
        private void UnRegisterOutOfScopeAction()
        {
            _outOfRangeAction = null;
        }
        
        public void Initialize(Vector3 position, Vector3 direction)
        {
            this.transform.position = position;
            this.transform.forward = direction;
            _initialPosition = position;
            _direction = direction;

            _verticalSpeed = speed;
            
        }

        private void OnDestroy()
        {
            UnRegisterOutOfScopeAction();
        }
        
        public Vector3 ElementWiseMultiply(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
    }
}