using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SakeShooter
{
    public class SakeGun : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private SakeShooterInputs _input;
        
        private void Start()
        {
            _input = GetComponent<SakeShooterInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
        }
        
        private void Update()
        {
            Fire();
        }
        
        private void Fire()
        {
            if (_input.fire)
            {
                Debug.Log("Fire");
            }
        }
    }
}
