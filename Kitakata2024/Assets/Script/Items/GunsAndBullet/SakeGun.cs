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
        [Tooltip("リロード（補充にかかる時間、小さいほど早くたまる")]
        public float reloadInterval = 0.05f;

        [SerializeField, Tooltip("打てる酒の量")] 
        private float _capacity = 30.0f;
        
        [SerializeField, Tooltip("マスに溜まる量")]
        private float _fillAmout = 10.0f;

        private float _currentAmount = 30.0f;

        // 一度の発火で減る量
        private const float OneShotAmount = 1.0f;
        
        // ここらの管理はビット演算を使ったほうがいいかもしれない（一つのみ1になるように) 
        private bool _canFire = true;
        private bool _canReload = true;
        private SakeShooterInputs _input;

        private const uint NumOfStates = 2;
        private bool[] _currentState = new bool[NumOfStates];

        private enum States
        {
            None,
            Fire,
            Fill
        };

        private void Start()
        {
            _input = player.GetComponent<SakeShooterInputs>();
            _canFire = true;
            _currentAmount = _capacity;
        }

        private void Update()
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            Debug.DrawRay(this.transform.position, ray.direction * 10, Color.red);
            
            if (_canFire) Fire();
        }

        private async void RefillBottle()
        {
            if (_currentAmount >= _capacity) _currentAmount = _capacity;
            _currentAmount += _fillAmout;
            // update UI

            await UniTask.Delay(TimeSpan.FromSeconds(reloadInterval));
        }

        private void ChangeState(bool currentState)
        {
            
        }

        private async void Fire()
        {
            if (_input.fire && _currentAmount > 0)
            {
                Debug.Log("fire");
                _canFire = false;

                var bullet = bulletObjectPool.GetBullet();
                bullet.Initialize(this.transform.position, this.transform.forward);
                
                await UniTask.Delay(TimeSpan.FromSeconds(fireInterval));
                _canFire = true;

                _currentAmount -= OneShotAmount;
            }
        }
    }
}