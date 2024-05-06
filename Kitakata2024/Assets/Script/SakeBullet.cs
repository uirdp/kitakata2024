using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        public float speed = 1.5f;
        public float gravity = 9.8f;

        private Vector3 _direction;

        private void Start()
        {
            _direction = this.transform.forward;
            _direction.x += 1.5f;
            _direction.z += 1.5f;
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
        
        public void SetInitialPositionAndDirection(Vector3 position, Vector3 direction)
        {
            this.transform.position = position;
            _direction = direction;
        }
        
    }
    
}