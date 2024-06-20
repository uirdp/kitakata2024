using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class MasuMovement : MonoBehaviour
    {
        private Masu _masu;
        private float _speed;
        private float _acceleration;
        private Vector3 _direction;
        private Vector3 _initialPosition;
        private float _distanceThreshold;
        private bool _isMoving;

        public void Initialize(Masu masu, float speed, float acceleration, Vector3 direction, Vector3 initialPosition, float distanceThreshold)
        {
            _masu = masu;
            _speed = speed;
            _acceleration = acceleration;
            _direction = direction;
            _initialPosition = initialPosition;
            _distanceThreshold = distanceThreshold;
            _isMoving = true;
        }

        private void Update()
        {
            if (_isMoving)
            {
                Move();
            }
        }

        private void Move()
        {
            transform.position += _direction * _speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, _initialPosition) > _distanceThreshold)
            {
                _isMoving = false;
            }
        }
    }
}