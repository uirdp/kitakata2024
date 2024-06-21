using System;
using SakeShooterSystems;
using UnityEngine;

namespace SakeShooterSystems
{
    public class BoxHitArea : MonoBehaviour, ICollider
    {
        public Vector3 size = new Vector3(2.0f, 2.0f, 2.0f);
        public Color gizmoColor = Color.yellow;

        private readonly ColliderShape _shape = ColliderShape.Box;
        private event Action OnHitDetected;

        public ColliderShape Shape => _shape;
        
        public bool IsEnable => enabled;
        public GameObject GameObject => gameObject;
        public ColliderSizeData Size => new ColliderSizeData { size = size };
        
        public int ColliderID { get; set; }
        
        private Action<ICollider> _onDestroyAction;
        private void Start()
        {
            
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, size);
        }

        public void RegisterOnHitDetected(Action action)
        {
            OnHitDetected += action;
        }

        public void UnregisterOnHitDetected()
        {
            OnHitDetected = null;
        }
        
        public void RegisterOnDestroyAction(Action<ICollider> action)
        {
            _onDestroyAction += action;
        }
        
        private void UnregisterOnDestroyAction()
        {
            _onDestroyAction = null;
        }
        
        private void OnDestroy()
        {
            _onDestroyAction?.Invoke(this);
        }

        
        public void InvokeOnHitDetected()
        {
            OnHitDetected?.Invoke();
        }
    }

}
