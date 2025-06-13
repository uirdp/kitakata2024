using UnityEngine;
using UnityEngine.Events;
using SakeShooterSystems;
using System;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;


namespace SakeShooter
{
    public class MasuResult : MonoBehaviour
    {
        public MMF_Player successFeedback;
        [SerializeField, Header("------ マス消滅時のイベント ------")]
        private GameEvent masuExitEvent = null;

        [SerializeField] private Masu masu = null;
       
        private Action<Masu> _onExitAction;

        private async UniTask PlayFeedbacks(MasuExitStatus status)
        {
            if(status == MasuExitStatus.Success)
            {
                await successFeedback.PlayFeedbacksTask();
            }
        }
        
        public async UniTask RaiseResultEvent(MasuExitStatus status)
        {
            var mov = masu.Movement;
            mov.Stop();
            
            await PlayFeedbacks(status);

			if (_onExitAction == null) gameObject.SetActive(false);
			else _onExitAction.Invoke(masu);　// オブジェクトプールに戻る
          
            masuExitEvent.Raise(status);
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
