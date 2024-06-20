using System;
using UnityEngine;

namespace SakeShooter
{
    // 名前をMasuに変えた方がいいかも？
    public class MasuStatus : MonoBehaviour
    {
        public float capacity = 100.0f;
        public GameObject Fluid;
        public float distanceThreshold = 10.0f;
        [SerializeField] public SakeShooterSystems.BoxCollider MasuCollider;
        
        public int ShaderPropertyID { get; set; }

        private float _initialCapacity;
        private float _currentAmount;
        private Vector3 _initialPosition;
        
    
        private event Action<float> OnFill;
        private event Action OnFullyFilled;
        private event Action<MasuStatus> _outOfRangeAction;

        private Material _material;
        public void Initialize()
        {
            // 容量と現在の量を、[-1, 1]の範囲に正規化する -> Shaderに渡すため (FluidのShaderのFillプロパティに渡す)
            _currentAmount = -1f;
            _initialCapacity = capacity;
            capacity /= capacity;
            
            OnFill += Fill;
            OnFullyFilled += FullEvent;
            
            // Shaderのプロパティ（Fill)をセットする
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
                // これアクションでやる必要あるのかな？
                OnFill?.Invoke(10.0f);
            }
            else
            {
                OnFullyFilled?.Invoke();
            }
        }
        
        public void RegisterOutOfRangeAction(Action<MasuStatus> action)
        {
            _outOfRangeAction += action;
        }
        
        private void UnRegisterOutOfRangeAction()
        {
            _outOfRangeAction = null;
        }
        
        private void CheckDistanceAndReturnToPool()
        {
            float distance = Vector3.Distance(transform.position, _initialPosition);
            if (distance > distanceThreshold)
            {
                _outOfRangeAction?.Invoke(this);
            }
        }

        private void Update()
        {
            CheckDistanceAndReturnToPool();
        }

        private void OnDestroy()
        {
            UnRegisterOutOfRangeAction();
        }
    }
}