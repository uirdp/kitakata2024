using SakeShooter;
using UnityEngine;
using UnityEngine.Pool;


namespace SakeShooter
{
    public class MasuObjectPool : MonoBehaviour
    {
        [Header("------ Models Setting ------")]
        public GameObject masuPrefab;

        [Header("------ Pool Settings ------")]
        public bool collectionChecks = true;
        public int maxPoolSize = 30;

        private IObjectPool<MasuStatus> _masuPool;

        private void Start()
        {
            _masuPool = new ObjectPool<MasuStatus>(CreateMasu, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject,
                collectionChecks, 10, maxPoolSize);
        }
        
        private MasuStatus CreateMasu()
        {
            var masu = Instantiate(masuPrefab);
            var masuStatus = masu.GetComponent<MasuStatus>();

            masuStatus.ShaderPropertyID = Shader.PropertyToID("_Fill");
            masuStatus.RegisterOutOfRangeAction(ReturnToPool);
            
            masuStatus.Initialize();
            masu.SetActive(false);

            return masuStatus;
        }
        
        private void OnTakeFromPool(MasuStatus masu)
        {
            masu.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(MasuStatus masu)
        {
            masu.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(MasuStatus masu)
        {
            Destroy(masu.gameObject);
        }
        
        private void ReturnToPool(MasuStatus masu)
        {
            _masuPool.Release(masu);
        }
        
        public MasuStatus GetMasu()
        {
            return _masuPool.Get();
        }

    }
    
}