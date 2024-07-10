using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


// ラッパークラスMasuをつくって、statusとmoveをもたせる
namespace SakeShooter
{
    // Masu Spawn Managerに改名すべし
    public class MasuManager : MonoBehaviour
    {
        public MasuObjectPool masuObjectPool;
        [Header("------ たわら ------")]
        public Masu tawara; // 今はまだ俵はマスに無理やりクラスをくっつけているだけなので、後で分離する
        public float tawaraSpeed = 2.0f;
        public float tawaraAcceleration = 1.03f;
        
        public Transform masuSpawnPoint_start;
        public Transform masuSpawnPoint_end;
        
        [Tooltip("升が流れる方向を指定")]
        public Vector3 masuMoveDirection;
        
        [Header("------ 升の速度と加速度 ------")]
        public float masuSpeed = 2.0f;
        public float masuAcceleration = 1.03f;
        
        [Header("------ 俵の生成間隔 ------")]
        public float tawaraSpawnInterval = 10.0f;
        // structでもいいかも
        private Vector3 _normalStart;
        private Vector3 _normalEnd;
        
        private float _maxWaitTime = 4.0f;
        private float _minWaitTime = 0.5f;
        private bool _canSpawn = false;
        private bool _canSpawnTawara = false;
        
        public void StartSpawning()
        {
            _canSpawn = true;
            _canSpawnTawara = true;
        }

        public void EndSpawning()
        {
            _canSpawn = false;
            _canSpawnTawara = false;
        }
        
        public void ChangeSpawnRate(float max, float min)
        {
            _maxWaitTime = max;
            _minWaitTime = min;
        }
        private void Start()
        {
            NormalizeDirection();
            tawara.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if(_canSpawn) SpawnMasuByTime(SetSpawnPoint(), masuSpeed, masuAcceleration);
            if(_canSpawnTawara) SpawnTawaraByTime(SetSpawnPoint(), tawaraSpeed, tawaraAcceleration);
        }

       
        private async void SpawnMasuByTime(Vector3 position, float speed, float acceleration)
        {
            SpawnMasu(position, speed, acceleration);
            _canSpawn = false;
            
            var waitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
       
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
        
        //　ここら辺はますとまとめたい
        private async void SpawnTawaraByTime(Vector3 position, float speed, float acceleration)
        {
            _canSpawnTawara = false;
       
            await UniTask.Delay(TimeSpan.FromSeconds(tawaraSpawnInterval));
            SpawnTawara(position, speed, acceleration);
            
            _canSpawnTawara = true;
        }
        
        private void SpawnTawara(Vector3 position, float speed, float acceleration)
        {
            Debug.Log("Spawn Tawara");
            tawara.gameObject.SetActive(true);   
            var masuStatus = tawara.Status;
            var masuMovement = tawara.Movement;
            
            masuStatus.Initialize(100.0f);
            masuMovement.Initialize(tawara, speed, acceleration, masuMoveDirection, position);
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