using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasuStatus : MonoBehaviour
{
    public float capacity = 100.0f;
    private float _currentAmount;
    
    private event Action<float> OnFill;
    private event Action OnFullyFilled;
    private void Initialize()
    {
        _currentAmount = 0f;
        OnFill += Fill;
        OnFullyFilled += FullEvent;
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
    private void OnHitDetected()
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
