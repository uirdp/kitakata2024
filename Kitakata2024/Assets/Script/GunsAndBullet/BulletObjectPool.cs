using UnityEngine;
using UnityEngine.Pool;
using SakeShooterSystems;

namespace SakeShooter
{
    public class BulletObjectPool : MonoBehaviour
    {
        
        public CollisionDetectionSystem collisionDetectionSystem;
        [Header("------ Models Setting ------")]
        public GameObject bulletPrefab;

        [Header("------ Pool Settings ------")]
        public bool collectionChecks = true;
        public int maxPoolSize = 30;

        private IObjectPool<SakeBullet> _bulletPool;

        private void Start()
        {
            _bulletPool = new ObjectPool<SakeBullet>(CreateBullet, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject,
                collectionChecks, 10, maxPoolSize);
        }

        // Bulletのプールをつくる
        private SakeBullet CreateBullet()
        {
            var bullet = Instantiate(bulletPrefab);
            var sakeBullet = bullet.GetComponent<SakeBullet>();
            var bulletCollider = bullet.GetComponent<ICollider>();
            
            // out of scope actionはSakeBulletが範囲外に出た時に行うアクション、プールに戻るように指定
            sakeBullet.RegisterOutOfScopeAction(ReturnToPool);
            // 衝突判定システムにコライダーを追加
            collisionDetectionSystem.AddColliderToList(bulletCollider);
            
            bullet.SetActive(false);
            return sakeBullet;
        }

        private void OnTakeFromPool(SakeBullet sakeBullet)
        {
            sakeBullet.gameObject.SetActive(true);
        }

        private void OnReturnToPool(SakeBullet sakeBullet)
        {
            sakeBullet.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(SakeBullet sakeBullet)
        {
            Destroy(sakeBullet.gameObject);
        }

        public SakeBullet GetBullet()
        {
            return _bulletPool.Get();
        }

        public void ReturnToPool(SakeBullet sakeBullet)
        {
            _bulletPool.Release(sakeBullet);
        }
    }
}