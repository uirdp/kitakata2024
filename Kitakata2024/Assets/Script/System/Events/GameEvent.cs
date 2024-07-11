using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SakeShooterSystems
{
    public enum MasuExitStatus
    {
        Success,
        Failure,
    }
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {

        private List<GameEventListener> listeners = new List<GameEventListener>();
        
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }
        public void Raise(MasuExitStatus status)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(status);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}