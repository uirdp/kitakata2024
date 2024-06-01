using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        [Header("------ Parameters ------")] public float speed = 4.5f;
        public float gravity = 0.8f;
        public float distanceThreshold;
        public Transform playerInitialPosition;
        public BulletObjectPool bulletObjectPool;

        private Vector3 _direction;

        private void Start()
        {
            _direction = this.transform.forward;
        }

        private void Update()
        {
            Move();
            CheckDistanceAndReturnToPool();
        }

        private void Move()
        {
            // Gravity
            _direction.y -= gravity * Time.deltaTime;
            this.transform.position += _direction * speed * Time.deltaTime;
        }

        private void CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, playerInitialPosition.position);
            if (distance > distanceThreshold)
            {
                bulletObjectPool.ReturnToPool(this.gameObject);

            }
        }
    }
}