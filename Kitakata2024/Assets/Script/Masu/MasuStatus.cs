using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasuStatus : MonoBehaviour
{
    public float capacity = 100.0f;
    private float _currentAmount;
    
    private event Action<float> OnFill; 
    private void Initialize()
    {
        _currentAmount = 0f;
        OnFill += Fill;
    }
    
    private void Fill(float amount)
    {
        _currentAmount += amount;
        if (_currentAmount > capacity)
        {
            _currentAmount = capacity;
        }
    }

    private void Start()
    {
        Initialize();
    }
    
    
}
