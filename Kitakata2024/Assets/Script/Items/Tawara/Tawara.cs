using System;
using MoreMountains.Feedbacks;
using SakeShooterSystems;
using UnityEngine;

// あまりよくない、時間ないので応急措置の実装

namespace SakeShooter
{
    public class Tawara : MonoBehaviour
    {
        public BoxHitArea tawaraCollider;
        public CollisionDetectionSystem collisionDetectionSystem;
        public GameEvent upgradeEvent;
        public MMF_Player upgradeFeedback;
        
        private bool _isUpgraded = false;
        
        private void OnEnable()
        {
            _isUpgraded = false;
        }

        private async void BroadCastUpgrade()
        {
           
            if (!_isUpgraded)
            {
                _isUpgraded = true;
                
                upgradeEvent.Raise();
                await upgradeFeedback.PlayFeedbacksTask();
                
                gameObject.SetActive(false);
            }
        }
        
        private void Start()
        {
            tawaraCollider.RegisterOnHitDetected(BroadCastUpgrade);
            
            collisionDetectionSystem.AddColliderToList(tawaraCollider);
            tawaraCollider.RegisterOnDestroyAction(collisionDetectionSystem.RemoveMasuCollider);
        }
    }
    
}
