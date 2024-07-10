using SakeShooterSystems;
using UnityEngine;

// あまりよくない、時間ないので応急措置の実装

namespace SakeShooter
{
    public class Tawara : MonoBehaviour
    {
        public BoxHitArea tawaraCollider;
        public MasuMovement tawaraMovement;

        public float tawaraSpeed;
        public float tawaraAcceleration;

        private void BroadCastUpgrade()
        {
            Debug.Log("update!");
        }
        
        private void Start()
        {
            tawaraCollider.RegisterOnHitDetected(BroadCastUpgrade);
            
            tawaraMovement.SetSpeed(tawaraSpeed);
            tawaraMovement.SetAcceleration(tawaraAcceleration);
        }
    }
    
}
