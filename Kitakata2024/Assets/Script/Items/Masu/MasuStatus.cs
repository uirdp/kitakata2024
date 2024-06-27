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
            // 容量と現在の量を、[-1, 1]の範囲に正規化する -> Shaderに渡すため (FluidのShaderのFillプロパティに渡す)
            _currentAmount = -1f;
            _capacity = capacity;
            _initialCapacity = _capacity;
            
            _capacity /= _capacity;
            
            // Shaderのプロパティ（Fill)をセットする
            // materialの取得は思いので、プールでやろう
            _material = fluid.GetComponent<Renderer>().material;
            _material.SetFloat(ShaderPropertyID, _currentAmount);
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
            // シェーダーの値を更新
            _material.SetFloat(ShaderPropertyID, _currentAmount);
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
                Fill(10.0f);
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
            
            //Debug.Log(Vector3.Dot(forward, toGoal));
            if (Vector3.Dot(-forward, toGoal) < 0)
            {
                await resultManager.RaiseResultEvent(_currentStatus);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Initialize();
            }
            
            CheckPosition();
        }
        
        
        
    }
}