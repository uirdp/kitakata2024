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
        public Vector3 goalPosition;
        
        private Action<Masu> _onExitAction;

        public UniTask RaiseResultEvent(MasuExitStatus status)
        {
            _onExitAction?.Invoke(masu);
            masuExitEvent.Raise(status);
            
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
