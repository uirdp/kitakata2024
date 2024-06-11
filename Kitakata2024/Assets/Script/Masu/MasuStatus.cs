using System;
using System.Collections;
using System.Collections.Generic;
using SakeShooterSystems;
using UnityEngine;

namespace SakeShooter
{
    public class MasuStatus : MonoBehaviour
    {
        public float capacity = 100.0f;
        [SerializeField] public SakeShooterSystems.BoxCollider MasuCollider;
        private float _currentAmount;
    
        private event Action<float> OnFill;
        private event Action OnFullyFilled;
        private void Initialize()
        {
            _currentAmount = 0f;
            OnFill += Fill;
            OnFullyFilled += FullEvent;
            
            MasuCollider.RegisterOnHitDetected(InvokeOnFill);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount;
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

        private void Start()
        {
            Initialize();
        }
    
    
    }
}
