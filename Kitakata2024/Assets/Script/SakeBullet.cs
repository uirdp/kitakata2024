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
        public float gravity = 9.8f;

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
            // Gravity
            _direction.y -= gravity * Time.deltaTime;
            this.transform.position += _direction * speed * Time.deltaTime;
        }
        
    }
    
}