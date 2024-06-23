using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class MasuMovement : MonoBehaviour
    {
        private Masu _masu;
        private float _speed = 1.0f;
        private float _acceleration;
        private Vector3 _direction;
        private Vector3 _initialPosition;

        public void Initialize(Masu masu, float speed, float acceleration, Vector3 direction, Vector3 initialPosition)
        {
            _masu = masu;
            _speed = speed;
            _acceleration = acceleration;
            _initialPosition = initialPosition;
            _direction = direction;
            
            this.transform.position = initialPosition;
            Debug.Log(initialPosition);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += -Vector3.forward * _speed * Time.deltaTime;
        }
    }
}