using System;
using UnityEngine;

namespace SakeShooter
{
    // 名前をMasuに変えた方がいいかも？
    public class MasuStatus : MonoBehaviour
    {
        public GameObject Fluid;
        [SerializeField] public SakeShooterSystems.BoxHitArea MasuCollider;
        
        public int ShaderPropertyID { get; set; }
        public MasuManager ManagerReference { get; set; }

        private float _initialCapacity;
        private float _currentAmount;
        
        private float _capacity;
        private Vector3 _initialPosition;
        
    
        private event Action<float> OnFill;
        private event Action OnFullyFilled;

        private Material _material;
        public void Initialize(float capacity = 100.0f)
        {
            // 容量と現在の量を、[-1, 1]の範囲に正規化する -> Shaderに渡すため (FluidのShaderのFillプロパティに渡す)
            _currentAmount = -1f;
            _capacity = capacity;
            _initialCapacity = _capacity;
            
            _capacity /= _capacity;
            
            OnFill += Fill;
            OnFullyFilled += FullEvent;
            
            //ShaderPropertyID = Shader.PropertyToID("_Fill");
            // Shaderのプロパティ（Fill)をセットする
            _material = Fluid.GetComponent<Renderer>().material;
            ShaderPropertyID = Shader.PropertyToID("_Fill");
            _material.SetFloat(ShaderPropertyID, _currentAmount);
            
            MasuCollider.RegisterOnHitDetected(InvokeOnFill);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
           
            _material.SetFloat(ShaderPropertyID, _currentAmount);
            Debug.Log("fill!");
        }

        private void FullEvent()
        {
            Debug.Log("Full!");
        }
      
        
        private void InvokeOnFill()
        {
            if(_currentAmount <= _capacity)
            {
                // これアクションでやる必要あるのかな？
                OnFill?.Invoke(10.0f);
            }
            else
            {
                OnFullyFilled?.Invoke();
            }
        }

        private void Start()
        {
            //Initialize(); //pcl
            Debug.Log(_currentAmount);
        }
        
    }
}