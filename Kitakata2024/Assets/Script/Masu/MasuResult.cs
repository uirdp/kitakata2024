using UnityEngine;
using UnityEngine.Events;
using SakeShooterSystems;
using System;
using Cysharp.Threading.Tasks;


namespace SakeShooter
{
    public class MasuResult : MonoBehaviour
    {
        [SerializeField, Header("------ マス消滅時のイベント ------")]
        private GameEvent masuExitEvent = null;

        [SerializeField] private Masu masu = null;
        
        private Action<Masu> _onExitAction;

        public UniTask RaiseSuccessEvent()
        {
            _onExitAction?.Invoke(masu);
            masuExitEvent.Raise(MasuExitStatus.Success);
            return UniTask.CompletedTask;
        }

        public UniTask RaiseFailureEvent()
        {
            masuExitEvent.Raise(MasuExitStatus.Failure);
            _onExitAction?.Invoke(masu);
            return UniTask.CompletedTask;
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
