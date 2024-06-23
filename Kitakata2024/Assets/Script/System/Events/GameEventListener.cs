using UnityEngine;
using UnityEngine.Events;

namespace SakeShooterSystems
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;

        public UnityEvent<MasuExitStatus> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(MasuExitStatus status)
        {
            Response.Invoke(status);
        }
    }
}
