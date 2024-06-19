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

        private IObjectPool<GameObject> _masuPool;

        private void Start()
        {
            _masuPool = new ObjectPool<GameObject>(CreateMasu, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject,
                collectionChecks, 10, maxPoolSize);
        }
        
        private GameObject CreateMasu()
        {
            var masu = Instantiate(masuPrefab);
            var masuStatus = masu.GetComponent<MasuStatus>();

            masuStatus.ShaderPropertyID = Shader.PropertyToID("_Fill");
            
            masuStatus.Initialize();
            masu.SetActive(false);

            return masu;
        }
        
        private void OnTakeFromPool(GameObject masu)
        {
            masu.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(GameObject masu)
        {
            masu.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject masu)
        {
            Destroy(masu.gameObject);
        }
        
        private void ReturnToPool(GameObject masu)
        {
            _masuPool.Release(masu);
        }

    }
    
}