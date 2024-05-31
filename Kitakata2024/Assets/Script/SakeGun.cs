using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
        [Header("------ Models Setting ------")]
        public GameObject bulletPrefab;
        public GameObject player;
        
        [Header("------ Parameter Settings ------")]
        [Tooltip("The time between each shot")]
        public float fireInterval = 0.05f;
        
        private bool _canFire = true;
        
        [Header("------ Pool Settings ------")]
        public bool collectionChecks = true;
        public int maxPoolSize = 30;
        
        private IObjectPool<GameObject> _bulletPool;
        private SakeShooterInputs _input;
        
        private void Start()
        {
            _input = player.GetComponent<SakeShooterInputs>();
            _bulletPool = new ObjectPool<GameObject>(CreateBullet, OnTakeFromPool, OnReturneToPool, OnDestroyPoolObject,
                                                     collectionChecks, 10, maxPoolSize);
        }
        
        private void Update()
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            Debug.DrawRay(this.transform.position, ray.direction * 10, Color.red);
            if(_canFire) Fire();
        }
        
        private async void Fire()
        {
            if (_input.fire)
            {
                _canFire = false;
                
                var bullet = _bulletPool.Get();
                InitializeBullet(bullet);

                await UniTask.Delay(TimeSpan.FromSeconds(fireInterval));
                _canFire = true;
            }
        }

        private void InitializeBullet(GameObject bullet)
        {
            bullet.transform.position = this.transform.position;
            bullet.transform.forward = this.transform.forward;
        }

        #region ObjectPool
            
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
            
            #endregion
    }
}
