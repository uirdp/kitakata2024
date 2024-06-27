using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
        [Header("------ Game Objects------")] public GameObject player;
        public BulletObjectPool bulletObjectPool;
        public UIController ui;

        [Header("------ Parameter Settings ------")] [Tooltip("The time between each shot")]
        public float fireInterval = 0.05f;
        [Tooltip("リロード（補充にかかる時間、小さいほど早くたまる")]
        public float reloadInterval = 10.0f;

        [SerializeField, Tooltip("打てる酒の量")] 
        private float _capacity = 30.0f;
        
        [SerializeField, Tooltip("マスに溜まる量")]
        private float _fillAmount = 1.0f;

        private float _currentAmount = 30.0f;

        // 一度の発火で減る量
        private const float OneShotAmount = 1.0f;
        
        // ここらの管理はビット演算を使ったほうがいいかもしれない（一つのみ1になるように) 
        private bool _canFire = true;
        private bool _canRefill = true;
        private SakeShooterInputs _input;

        private const uint NumOfStates = 2;
        private States _currentState = States.None;

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
            
            ChangeState();
            
            if (_canFire && _currentState == States.Fire) Fire();
            if (_canRefill && _currentState == States.Fill) RefillBottle();
        }

        private async void RefillBottle()
        {
            if (_input.refill)
            {
                _canRefill = false;
                _currentAmount += _fillAmount;
                if (_currentAmount >= _capacity) _currentAmount = _capacity;
                
                ui.UpdateSakeAmount(_currentAmount);
                await UniTask.Delay(TimeSpan.FromSeconds(reloadInterval));
                _canRefill = true;
            }
        }

        // 状態管理クラスを別に作ってもいいかも、あともっと簡素な書き方ができる気がする
        private void ChangeState()
        {
            if (_input.fire) _currentState = States.Fire;
            if (_input.refill) _currentState = States.Fill;
            
            // もしどのボタンも押されていなければ... もっといい書き方あるかも
            if (!_input.refill && !_input.fire) _currentState = States.None;
        }

        private async void Fire()
        {
            if (_input.fire && _currentAmount > 0)
            {
                _canFire = false;

                var bullet = bulletObjectPool.GetBullet();
                bullet.Initialize(this.transform.position, this.transform.forward);
                
                await UniTask.Delay(TimeSpan.FromSeconds(fireInterval));
                _canFire = true;

                _currentAmount -= OneShotAmount;
                ui.UpdateSakeAmount(_currentAmount);
            }
        }
    }
}