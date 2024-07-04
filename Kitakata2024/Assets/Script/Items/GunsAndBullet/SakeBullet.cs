using System;
using UnityEngine;
using SphereCollider = SakeShooterSystems.SphereCollider;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        [Header("------ Parameters ------")]
        public float speed = 6.5f;
        public float gravity = 0.8f;
        public float distanceThreshold = 20.0f;
        [Header("大きくすると玉がより下に飛ぶように")]
        public float directionOffset = 0.5f;

        public SakeShooterSystems.SphereCollider collider;

        private Vector3 _direction;
        private Vector3 _initialPosition;
        private int _colliderID;
        private Action<SakeBullet> _outOfRangeAction;
        
        public int ColliderID
        {
            get;
            set;
        }

        private void Start()
        {
            collider.RegisterOnHitDetected(ReturnToPool);
        }
        private void Update()
        {
            Move();
            CheckDistanceAndReturnToPool();
        }

        private void Move()
        {
            // non-realistic, but cheap ya know
            _direction.y -= gravity * Time.deltaTime;
            this.transform.position += _direction * speed * Time.deltaTime;
        }
        
        private void ReturnToPool()
        {
            _outOfRangeAction?.Invoke(this);
        }
        // Check if the bullet is out of range, if so, return to pool
        private void CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, _initialPosition);
            if (distance > distanceThreshold)
            {
              ReturnToPool();
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
        
        
    
        
        // Initialize the bullet with a position and direction
        public void Initialize(Vector3 position, Vector3 direction)
        {
            this.transform.position = position;
            direction.y -= directionOffset;
            this.transform.forward = direction;
            
            _initialPosition = position;
            _direction = direction;
            
        }

        private void OnDestroy()
        {
            UnRegisterOutOfScopeAction();
        }
        
    }
}