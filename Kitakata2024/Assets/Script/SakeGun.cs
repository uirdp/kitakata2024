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

        public bool collectionChecks = true;
        public int maxPoolSize = 30;
        
        private IObjectPool<GameObject> _bulletPool;
        
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private SakeShooterInputs _input;
        
        private void Start()
        {
            _input = GetComponent<SakeShooterInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            
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
                Debug.Log("Fire");

                var bullet = _bulletPool.Get();
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
