using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ラッパークラスMasuをつくって、statusとmoveをもたせる
namespace SakeShooter
{
    public class MasuManager : MonoBehaviour
    {
        public MasuObjectPool masuObjectPool;
        
        [Tooltip("升が流れる方向を指定")]
        public Vector3 masuMoveDirection;


        private void Start()
        {
            NormalizeDirection();
        }
        private void Update()
        {
            if(Input.GetKeyDown("space"))
            {
                SpawnMasu(transform.position, 0.0f, 1.0f);
            }   
        }

        private void SpawnMasu(Vector3 position, float speed, float acceleration)
        {
            var masu = masuObjectPool.GetMasu();
            var masuStatus = masu.Status;
            var masuMovement = masu.Movement;
            
            masuStatus.Initialize(100.0f);
            masuMovement.Initialize(masu, speed, acceleration, masuMoveDirection, position);
        }
        
        private void NormalizeDirection()
        {
            masuMoveDirection.Normalize();
        }
      
        void OnDrawGizmos()
        {
            // Draw a yellow line from the origin of the object in the direction of masuMoveDirection
            Debug.DrawRay(transform.position, masuMoveDirection, Color.yellow);
        }
        
        
    }
}