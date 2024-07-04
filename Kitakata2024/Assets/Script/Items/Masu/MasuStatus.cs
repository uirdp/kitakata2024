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
        public MMF_Player successFeedback;
        
        public Vector3 goalPosition;
        
        public Transform fluidTransform;
        [Tooltip("お酒の高さ-------------")]
        public float fluidMinHeight;
        public float fluidMaxHeight;
        
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
            _currentStatus = MasuExitStatus.Failure;
            
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

        private void Full()
        {
            _currentStatus = MasuExitStatus.Success;
        }
      
        // なまえよくないとおもう
        private void InvokeOnFill()
        {
            if(_currentAmount <= _capacity)
            {
                Fill(6.0f);
            }
            else
            {
                Full();
            }
        }

        private void PlayFeedback()
        {
            switch (_currentStatus)
            {
                case MasuExitStatus.Success:
                    successFeedback.PlayFeedbacks();
                    break;
                
                case MasuExitStatus.Failure:
                    Debug.Log("Failure");
                    break;
            }
        }
        // MMF_Playerから呼ばれます
        public async void RaiseResultEvent()
        {
            await resultManager.RaiseResultEvent(_currentStatus);
        }
        private void CheckPosition()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toGoal = goalPosition - transform.position;
            
            if (Vector3.Dot(-forward, toGoal) < 0)
            {
               PlayFeedback();
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