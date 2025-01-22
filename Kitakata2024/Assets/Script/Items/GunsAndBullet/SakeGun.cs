using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using MoreMountains.Feedbacks;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
        [Header("------ Game Objects------")] public GameObject player;
        public BulletObjectPool bulletObjectPool;
        public UIController ui;
        
        [Header("------ 着せ替え ------")]
        public List<Material> materials;

        [Header("------ Parameter Settings ------")] [Tooltip("The time between each shot")]
        public float fireInterval = 0.02f;
        [Tooltip("リロード（補充にかかる時間、小さいほど早くたまる")]
        public float reloadInterval = 5.0f;
        
        private float _initialFireInterval = 0.1f;
        private float _initialReloadInterval = 0.08f;
        
        [Tooltip("アップグレード時に上がる数値")]
        public float upgradeFireInterval = 0.005f;
        public float upgradeReloadInterval = 0.5f;
        public MMF_Player upgradeFeedback;

        [SerializeField, Tooltip("打てる酒の量")] 
        private float _capacity = 50.0f;
        
        [SerializeField, Tooltip("マスに溜まる量")]
        private float _fillAmount = 2.0f;

        private float _currentAmount = 50.0f;

        // 一度の発火で減る量
        private const float OneShotAmount = 1.0f;
        
        // ここらの管理はビット演算を使ったほうがいいかもしれない（一つのみ1になるように) 
        private bool _canFire = true;
        private bool _canRefill = true;
        private SakeShooterInputs _input;

        private const uint NumOfStates = 2;
        private States _currentState = States.None;
        
        private Renderer _renderer;
        private Material _currentMaterial;
        private int _currentMaterialIndex = 0;

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

            
            // Initialize the renderer
            _renderer = GetComponent<Renderer>();
            _currentMaterialIndex = 0;
            // Set the initial material
            _renderer.material = materials[_currentMaterialIndex];
        }

        private void Update()
        {
            
            ChangeMaterial();
            ChangeState();
            
            if (_canFire && _currentState == States.Fire) Fire();
            if (_canRefill && _currentState == States.Fill) RefillBottle();
        }
        
        public void ResetUpgradeStatus()
        {
            fireInterval = _initialFireInterval;
            reloadInterval = _initialReloadInterval;
        }
        public void Upgrade()
        { 
            upgradeFeedback.PlayFeedbacks();
            float upgradedFireInterval = fireInterval - upgradeFireInterval;
            float upgradedReloadInterval = reloadInterval - upgradeReloadInterval;
            
            if(upgradedFireInterval > 0f)
            {
                fireInterval = upgradedFireInterval;
            }
            
            if(upgradedReloadInterval > 0f)
            {
                reloadInterval = upgradedReloadInterval;
            }
            
            if(upgradedFireInterval <= 0f && upgradedReloadInterval <= 0f)
            {
                Debug.Log("これ以上アップグレードできません");
            }
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
        
        private void ChangeMaterial()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                
                    _currentMaterialIndex++;
                    if (_currentMaterialIndex >= materials.Count) _currentMaterialIndex = 0;
                    // Update the material of the renderer
                    _renderer.material = materials[_currentMaterialIndex];
                
            }
        }
    }
}