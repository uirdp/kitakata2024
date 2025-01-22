using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace SakeShooterSystems
{
    // ここももうちょっときれいにしてね
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        
        [Header("------ 銃アップグレード時のイベント ------")]
        public UnityEvent upgradeResponse;
        [Header("------ マス消滅時のイベント ------")]
        public UnityEvent<MasuExitStatus> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        
        public void OnEventRaised()
        {
            upgradeResponse?.Invoke();
        }
        
        public void OnEventRaised(MasuExitStatus status)
        {
            Response?.Invoke(status);
        }
    }
}
