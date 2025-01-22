using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


// ラッパークラスMasuをつくって、statusとmoveをもたせる
namespace SakeShooter
{
    // Masu Spawn Managerに改名すべし
    public class MasuSpawnManager : MonoBehaviour
    {
        public MasuObjectPool masuObjectPool;
        [Header("------ たわら ------")]
        public Masu tawara; // 今はまだ俵はマスに無理やりクラスをくっつけているだけなので、後で分離する
        
        [Header("------ マスの生成範囲 ------")]
        public Transform masuSpawnPoint_start;
        public Transform masuSpawnPoint_end;
        
        [Tooltip("升が流れる方向を指定"), HideInInspector]
        public Vector3 masuMoveDirection;
        
        // ---------------- Parameters ------------------
        [HideInInspector]
        public float tawaraSpeed = 2.0f;
        public float tawaraAcceleration = 1.30f;
        
        [HideInInspector]
        public float masuSpeed = 2.0f;
        public float masuAcceleration = 1.03f;
        
        [HideInInspector]
        public float tawaraSpawnInterval = 8.0f;
        
        [HideInInspector]
        public float fillAmount = 5.0f;
        // ----------------------------------------------
        
        // structでもいいかも
        private Vector3 _normalStart;
        private Vector3 _normalEnd;
        
        private float _maxWaitTime = 4.0f;
        private float _minWaitTime = 0.5f;
        
        private bool _endSpawn = false;
        
        private bool _canSpawn = false;
        private bool _canSpawnTawara = false;
        
        public void StartSpawning()
        {
            _endSpawn = false;
         
            _canSpawn = true;
            _canSpawnTawara = true;
        }

        public void EndSpawning()
        { 
            _canSpawn = false;
            _canSpawnTawara = false;
            _endSpawn = true;
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
            if(_endSpawn)
            {
                _canSpawn = false;
                _canSpawnTawara = false;
            }
            
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
            
            //　ここ怖いけど応急措置
            masuStatus.fillAmount = fillAmount;
            
            masuStatus.Initialize(60.0f);
            masuMovement.Initialize(masu, speed, acceleration, masuMoveDirection, position);
        }
        
        //　ここら辺はますとまとめたい
        private async void SpawnTawaraByTime(Vector3 position, float speed, float acceleration)
        {
            _canSpawnTawara = false;
            
            float spawnTime = UnityEngine.Random.Range(tawaraSpawnInterval, tawaraSpawnInterval + 2.0f);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnTime));
            SpawnTawara(position, speed, acceleration);
            
            _canSpawnTawara = true;
        }
        
        private void SpawnTawara(Vector3 position, float speed, float acceleration)
        {
            tawara.gameObject.SetActive(true);
            
            var masuStatus = tawara.Status;
            var masuMovement = tawara.Movement;
            
            masuStatus.Initialize(10000.0f);
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