using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        public float directionX = 1.0f;
        public float directionY = 20.0f;
        public float directionZ = 3.0f;

        public float speed = 1.0f;

        private Vector3 _direction;

        private void Start()
        {
            _direction = new Vector3(directionX, directionY, directionZ);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            CalculateGravity();
            this.transform.position += _direction * speed * Time.deltaTime;
        }

        private void CalculateGravity()
        {
            // Gravity
            _direction.y -= 9.8f * Time.deltaTime;
        }
    }
    
}