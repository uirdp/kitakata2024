using SakeShooterSystems;
using UnityEngine;

// あまりよくない、時間ないので応急措置の実装

namespace SakeShooter
{
    public class Tawara : MonoBehaviour
    {
        public BoxHitArea tawaraCollider;
        public CollisionDetectionSystem collisionDetectionSystem;

        private void BroadCastUpgrade()
        {
            Debug.Log("ugrade!");
        }
        
        private void Start()
        {
            tawaraCollider.RegisterOnHitDetected(BroadCastUpgrade);
            
            collisionDetectionSystem.AddColliderToList(tawaraCollider);
            tawaraCollider.RegisterOnDestroyAction(collisionDetectionSystem.RemoveMasuCollider);
        }
    }
    
}
