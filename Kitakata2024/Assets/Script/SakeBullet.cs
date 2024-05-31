using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class SakeBullet : MonoBehaviour
    {
        public float speed = 4.5f;
        public float gravity = 0.8f;

        private Vector3 _direction;

        private void Start()
        {
            _direction = this.transform.forward;
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