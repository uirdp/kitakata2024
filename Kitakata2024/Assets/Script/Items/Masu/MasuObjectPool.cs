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
         
            var col = masu.Collider;
            // 衝突判定システムにコライダーを追加
            collisionDetectionSystem.AddColliderToList(col);
            //　衝突判定システムに、オブジェクト破壊時コライダーをリストから除外するように登録
            col.RegisterOnDestroyAction(collisionDetectionSystem.RemoveMasuCollider);
            
            var masuStatus = masu.Status;
            // Shaderに渡すプロパティのIDを取得
            masuStatus.ShaderPropertyID = _shaderPropertyID;
            
            var res = masu.Result;
            res.RegisterOnExitAction(ReturnToPool);
            
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
            Debug.Log("here");
            _masuPool.Release(masu);
        }
        
        public Masu GetMasu()
        {
            return _masuPool.Get();
        }

    }
    
}