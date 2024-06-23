using System;
using UnityEngine;
using SakeShooterSystems;

namespace SakeShooterSystems{

    public class SphereCollider : MonoBehaviour, ICollider
    {
        public float radius = 3.0f;
        public Color gizmoColor = Color.red;

        private readonly ColliderShape _shape = ColliderShape.Sphere;
        private event Action OnHitDetected;

        public ColliderShape Shape => _shape;
        public bool IsEnable => enabled;
        public GameObject GameObject => gameObject;
        public ColliderSizeData Size => new ColliderSizeData { radius = radius };
        
        public int ColliderID { get; set; }
        
        private Action<ICollider> _onDestroyAction;

        public void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, radius);
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