using UnityEngine;
using UnityEngine.Events;
using SakeShooterSystems;
using System;

namespace SakeShooter
{
    public class MasuResult : MonoBehaviour
    {
        [SerializeField, Header("------ マス消滅時のイベント ------")]
        private GameEvent masuExitEvent = null;

        [SerializeField] private Masu masu = null;
        
        private Action<Masu> _onExitAction;

        public void RaiseSuccessEvent()
        {
            masuExitEvent.Raise(MasuExitStatus.Success);
            _onExitAction?.Invoke(masu);
        }

        public void RaiseFailureEvent()
        {
            masuExitEvent.Raise(MasuExitStatus.Failure);
            _onExitAction?.Invoke(masu);
        }
        
        public void RegisterOnExitAction(Action<Masu> action)
        {
            _onExitAction += action;
        }
        
        public void UnRegisterOnExitAction()
        {
            _onExitAction = null;
        }
        
        private void OnDestroy()
        {
            UnRegisterOnExitAction();
        }
    }
}
