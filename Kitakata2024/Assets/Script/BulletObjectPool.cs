using UnityEngine;
using UnityEngine.Pool;

namespace SakeShooter
{
    public class BulletObjectPool : MonoBehaviour
    {
        [Header("------ Models Setting ------")]
        public GameObject bulletPrefab;

        [Header("------ Pool Settings ------")]
        public bool collectionChecks = true;
        public int maxPoolSize = 30;

        private IObjectPool<GameObject> _bulletPool;

        private void Start()
        {
            _bulletPool = new ObjectPool<GameObject>(CreateBullet, OnTakeFromPool, OnReturneToPool, OnDestroyPoolObject,
                collectionChecks, 10, maxPoolSize);
        }

        private GameObject CreateBullet()
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            return bullet;
        }

        private void OnTakeFromPool(GameObject bullet)
        {
            bullet.SetActive(true);
        }

        private void OnReturneToPool(GameObject bullet)
        {
            bullet.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject bullet)
        {
            Destroy(bullet);
        }

        public GameObject GetBullet()
        {
            return _bulletPool.Get();
        }
        
        public void ReturnToPool(GameObject bullet)
        {
            _bulletPool.Release(bullet);
        }
    }
}