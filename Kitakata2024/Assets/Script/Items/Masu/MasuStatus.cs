using System;
using System.ComponentModel.Design;
using UnityEngine;
using SakeShooterSystems;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace SakeShooter
{
    // 名前をMasuに変えた方がいいかも？
    public class MasuStatus : MonoBehaviour
    {
        public GameObject fluid;
        [SerializeField] public SakeShooterSystems.BoxHitArea masuCollider;
        public MasuResult resultManager;
        public Vector3 goalPosition;
        
        public Transform fluidTransform;
        [Tooltip("お酒の最大の高さ")]
        public float fluidMinHeight;
        public float fluidMaxHeight;
        
        
        public int ShaderPropertyID { get; set; }
        
        private float _initialCapacity;
        private float _currentAmount;
        
        private float _capacity;
        private Vector3 _initialPosition;

        private Material _material;

        private MasuExitStatus _currentStatus = MasuExitStatus.Failure;

        public void Start()
        {
            masuCollider.RegisterOnHitDetected(InvokeOnFill);
        }

        public void Initialize(float capacity = 100.0f)
        {
            _currentAmount = 0f;
            _capacity = capacity;
            
            _initialCapacity = capacity;
            _capacity /= capacity; // [0, 1]に正規化
            
            Vector3 position = fluidTransform.position;
            position.y = fluidMinHeight;
            fluid.transform.position = position;
            
            // ちょっとよくわからんなった
            // Shaderのプロパティ（Fill)をセットする
            // materialの取得は思いので、プールでやろう
            //_material = fluid.GetComponent<Renderer>().material;
            //_material.SetFloat(ShaderPropertyID, _currentAmount);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
            // シェーダーの値を更新
            //_material.SetFloat(ShaderPropertyID, _currentAmount);
            
            Vector3 position = fluidTransform.position;
            position.y = Mathf.Lerp(fluidMinHeight, fluidMaxHeight, _currentAmount);
            fluidTransform.position = position;
        }

        private void Full()
        {
            _currentStatus = MasuExitStatus.Success;
        }
      
        // なまえよくないとおもう
        private void InvokeOnFill()
        {
            if(_currentAmount <= _capacity)
            {
                Fill(6.0f);
            }
            else
            {
                Full();
            }
        }

        private async void CheckPosition()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toGoal = goalPosition - transform.position;
            
            if (Vector3.Dot(-forward, toGoal) < 0)
            {
                await resultManager.RaiseResultEvent(_currentStatus);
            }
        }

        private void Update()
        {
            CheckPosition();
        }
        
        
        
    }
}