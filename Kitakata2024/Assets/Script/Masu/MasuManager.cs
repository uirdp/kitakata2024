using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


// ラッパークラスMasuをつくって、statusとmoveをもたせる
namespace SakeShooter
{
    public class MasuManager : MonoBehaviour
    {
        public MasuObjectPool masuObjectPool;
        
        public Transform masuSpawnPoint_start;
        public Transform masuSpawnPoint_end;
        
        [Tooltip("升が流れる方向を指定")]
        public Vector3 masuMoveDirection;
        
        // structでもいいかも
        private Vector3 _normalStart;
        private Vector3 _normalEnd;
        
        private void Start()
        {
            NormalizeDirection();
        }
        private void Update()
        {
            if(Input.GetKeyDown("space"))
            {
                SpawnMasu(SetSpawnPoint(), 5.0f, 1.0f);
            }   
        }
        
        private Vector3 SetSpawnPoint()
        {
            return Vector3.Lerp(masuSpawnPoint_start.position, masuSpawnPoint_end.position, UnityEngine.Random.Range(0.0f, 1.0f));
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
        
        /*private void CreateNormal()
        {
            Vector3 origin = Vector3.zero - masuMoveDirection;
            
            float dx = origin.x / origin.z;
            float dz = origin.z / origin.x;
            
            _normalStart = new Vector3(origin.z + (dx * d), origin.y, 
                                    -origin.x + (dz * d));
            
            _normalEnd = new Vector3(-origin.z + (-dx * d), origin.y, 
                origin.x + (dz * d));
            
        }*/
        

        private void DrawLine()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            
            lineRenderer.SetPosition(0, _normalStart);
            lineRenderer.SetPosition(1, _normalEnd);
        }

        void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, masuMoveDirection, Color.yellow);
            //DrawLine();
        }
        
        
    }
}