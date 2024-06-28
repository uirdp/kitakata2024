using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
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
        
        [SerializeField, Header("------ 升の生成間隔の上限と加減 ------")]
        public float _MaxWaitTime = 7.0f;
        public float _MinWaitTime = 0.5f;
        private bool _canSpawn = false;


        public void GameStart()
        {
            _canSpawn = true;
        }

        public void GameEnd()
        {
            _canSpawn = false;
        }
        private void Start()
        {
            NormalizeDirection();
        }
        private void Update()
        {
            if(_canSpawn) SpawnMasuByTime(SetSpawnPoint(), 3.5f, 1.05f);
        }
        
        private async void SpawnMasuByTime(Vector3 position, float speed, float acceleration)
        {
            SpawnMasu(position, speed, acceleration);
            _canSpawn = false;
            
            var waitTime = UnityEngine.Random.Range(_MinWaitTime, _MaxWaitTime);
       
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            _canSpawn = true;
        }
        
        private Vector3 SetSpawnPoint()
        {
            return Vector3.Lerp(masuSpawnPoint_start.position, 
                masuSpawnPoint_end.position, UnityEngine.Random.Range(0.0f, 1.0f));
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
            Debug.DrawRay(transform.position, masuMoveDirection, Color.yellow);
        }
        
    }
}