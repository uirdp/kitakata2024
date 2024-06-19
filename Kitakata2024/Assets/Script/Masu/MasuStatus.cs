using System;
using UnityEngine;

namespace SakeShooter
{
    public class MasuStatus : MonoBehaviour
    {
        public float capacity = 100.0f;
        [SerializeField] public SakeShooterSystems.BoxCollider MasuCollider;
        public GameObject Fluid;
        
        public int ShaderPropertyID { get; set; }

        private float _initialCapacity;
        private float _currentAmount;
    
        private event Action<float> OnFill;
        private event Action OnFullyFilled;

        private Material _material;
        public void Initialize()
        {
            _currentAmount = 0f;
            OnFill += Fill;
            OnFullyFilled += FullEvent;

            _material = Fluid.GetComponent<Renderer>().material;
            _material.SetFloat(ShaderPropertyID, _currentAmount);
            
            MasuCollider.RegisterOnHitDetected(InvokeOnFill);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
           
            _material.SetFloat(ShaderPropertyID, _currentAmount);
            Debug.Log(amount);
        }

        private void FullEvent()
        {
            Debug.Log("Full!");
        }

        //あとはこいつをColliderに登録するだけ！！！
        private void InvokeOnFill()
        {
            if(_currentAmount < capacity)
            {
                OnFill?.Invoke(10.0f);
            }
            else
            {
                OnFullyFilled?.Invoke();
            }
        }
    
    
    }
}