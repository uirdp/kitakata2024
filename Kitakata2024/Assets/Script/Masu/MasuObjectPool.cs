using SakeShooter;
using UnityEngine;
using UnityEngine.Pool;
using SakeShooterSystems;

namespace SakeShooter
{
    public class MasuObjectPool : MonoBehaviour
    {
        public CollisionDetectionSystem collisionDetectionSystem;
        [Header("------ Models Setting ------")]
        public GameObject masuPrefab;

        [Header("------ Pool Settings ------")]
        public bool collectionChecks = true;
        public int maxPoolSize = 30;

        private IObjectPool<Masu> _masuPool;
        private int _shaderPropertyID;

        private void Start()
        {
            _masuPool = new ObjectPool<Masu>(CreateMasu, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject,
                collectionChecks, 10, maxPoolSize);
            
            _shaderPropertyID = Shader.PropertyToID("_Fill");
        }
        
        private Masu CreateMasu()
        {
            var masuGo = Instantiate(masuPrefab);
            var masu = masuGo.GetComponent<Masu>();
            var col = masuGo.GetComponent<ICollider>();
            
            
            var masuStatus = masu.Status;
            var masuMovement = masu.Movement;
            
            // Shaderに渡すプロパティのIDを取得
            masuStatus.ShaderPropertyID = _shaderPropertyID;
            // 升が画面外に出た時の処理を登録
            masu.RegisterOutOfRangeAction(ReturnToPool);
            
            collisionDetectionSystem.AddColliderToList(col);
            Debug.Log(col.Size.size);
            
            masuStatus.Initialize();
            masuGo.SetActive(false);

            return masu;
        }
        
        private void OnTakeFromPool(Masu masu)
        {
            masu.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(Masu masu)
        {
            masu.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(Masu masu)
        {
            Destroy(masu.gameObject);
        }
        
        private void ReturnToPool(Masu masu)
        {
            _masuPool.Release(masu);
        }
        
        public Masu GetMasu()
        {
            return _masuPool.Get();
        }

    }
    
}