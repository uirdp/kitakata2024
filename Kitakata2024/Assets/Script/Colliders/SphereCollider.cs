using System;
using UnityEngine;
using SakeShooterSystems;

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
}
