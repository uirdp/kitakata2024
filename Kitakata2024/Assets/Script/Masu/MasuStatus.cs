using System;
using UnityEngine;
using SakeShooterSystems;

namespace SakeShooter
{
    // 名前をMasuに変えた方がいいかも？
    public class MasuStatus : MonoBehaviour
    {
        public GameObject Fluid;
        [SerializeField] public SakeShooterSystems.BoxHitArea MasuCollider;
        public MasuResult resultManager;
        
        public int ShaderPropertyID { get; set; }
        
        private float _initialCapacity;
        private float _currentAmount;
        
        private float _capacity;
        private Vector3 _initialPosition;

        private Material _material;
        public void Initialize(float capacity = 100.0f)
        {
            // 容量と現在の量を、[-1, 1]の範囲に正規化する -> Shaderに渡すため (FluidのShaderのFillプロパティに渡す)
            _currentAmount = -1f;
            _capacity = capacity;
            _initialCapacity = _capacity;
            
            _capacity /= _capacity;
            
            // Shaderのプロパティ（Fill)をセットする
            // materialの取得は思いので、プールでやろう
            _material = Fluid.GetComponent<Renderer>().material;
            ShaderPropertyID = Shader.PropertyToID("_Fill");
            _material.SetFloat(ShaderPropertyID, _currentAmount);
            
            MasuCollider.RegisterOnHitDetected(InvokeOnFill);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
           
            _material.SetFloat(ShaderPropertyID, _currentAmount);
        }

        private void FullEvent()
        {
            resultManager.RaiseSuccessEvent();
        }
      
        
        private void InvokeOnFill()
        {
            if(_currentAmount <= _capacity)
            {
                Fill(10.0f);
            }
            else
            {
                FullEvent();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Initialize();
            }
        }
        
        
        
    }
}