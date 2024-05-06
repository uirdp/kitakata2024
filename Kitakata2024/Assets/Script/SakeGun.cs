using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public GameObject player;

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
            Fire();
        }
        
        private void Fire()
        {
            if (_input.fire)
            {
                var bullet = _bulletPool.Get();
                bullet.transform.position = this.transform.position;
                bullet.transform.forward = this.transform.forward;
            }
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
