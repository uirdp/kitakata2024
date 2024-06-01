using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
        [Header("------ Game Objects------")] public GameObject player;
        public BulletObjectPool bulletObjectPool;

        [Header("------ Parameter Settings ------")] [Tooltip("The time between each shot")]
        public float fireInterval = 0.05f;

        private bool _canFire = true;
        private SakeShooterInputs _input;

        private void Start()
        {
            //Kind of like importing input setting
            _input = player.GetComponent<SakeShooterInputs>();
        }

        private void Update()
        {
            //Skip ray part, it's for debugging
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            Debug.DrawRay(this.transform.position, ray.direction * 10, Color.red);
            
            if (_canFire) Fire();
        }

        private async void Fire()
        {
            if (_input.fire)
            {
                _canFire = false;

                var bullet = bulletObjectPool.GetBullet();
                bullet.Initialize(this.transform.position, this.transform.forward);
                
                await UniTask.Delay(TimeSpan.FromSeconds(fireInterval));
                _canFire = true;
            }
        }


    }
}