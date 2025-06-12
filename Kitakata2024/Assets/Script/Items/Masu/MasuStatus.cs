using UnityEngine;
using SakeShooterSystems;
using MoreMountains.Feedbacks;

namespace SakeShooter
{
    // 名前をMasuに変えた方がいいかも？
    public class MasuStatus : MonoBehaviour
    {
        public GameObject fluid;
        [SerializeField] public BoxHitArea masuCollider;
        public MasuResult resultManager;
        public MMF_Player fullFeedback;
        
        public Vector3 goalPosition;
        
        public Transform fluidTransform;
        [Tooltip("お酒の高さ-------------")]
        public float fluidMinHeight;
        public float fluidMaxHeight;
        
        public float fillAmount;
        
        private float _initialCapacity;
        private float _currentAmount;
        
        private float _capacity;
        private Vector3 _initialPosition;

        private Material _material;

        private MasuExitStatus _currentStatus = MasuExitStatus.Failure;

        private bool _hasReachedGoal = false;
        private bool _isPlayingFeedback = false; 

        public void Start()
        {
            masuCollider.RegisterOnHitDetected(OnHitDetected);
        }

        public void Initialize(float capacity = 100.0f)
        {
            _currentStatus = MasuExitStatus.Failure;

            _hasReachedGoal = false;
            _currentAmount = 0f;
            _capacity = capacity;
            
            _initialCapacity = capacity;
            _capacity /= capacity; // [0, 1]に正規化
            
            Vector3 position = fluidTransform.position;
            position.y = fluidMinHeight;
            fluid.transform.position = position;
        }
    
        private void Fill(float amount)
        {
            _currentAmount += amount / _initialCapacity;
            
            Vector3 position = fluidTransform.position;
            position.y = Mathf.Lerp(fluidMinHeight, fluidMaxHeight, _currentAmount);
            fluidTransform.position = position;
        }

        private async void Full()
        {
            // 満杯時の演出
            _isPlayingFeedback = true;
            _currentStatus = MasuExitStatus.Success;
            
            await fullFeedback?.PlayFeedbacksTask();
            _isPlayingFeedback = false;
        }
      
        // 弾が当たった時の処理
        private void OnHitDetected()
        {
            if(_currentAmount <= _capacity)
            {
                Fill(fillAmount);
            }
            else
            {
                if(_currentStatus == MasuExitStatus.Failure) Full();
            }
        }
        private async void CheckPosition()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toGoal = goalPosition - transform.position;
            
            if (Vector3.Dot(-forward, toGoal) < 0)
            {
                _hasReachedGoal = true;
                await resultManager.RaiseResultEvent(_currentStatus);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Initialize();
            }
            if(!_hasReachedGoal && !_isPlayingFeedback) CheckPosition();
        }
        
        
        
    }
}