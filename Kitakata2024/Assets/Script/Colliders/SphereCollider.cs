using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SakeShooterSystems;

public class SphereCollider : MonoBehaviour, ICollisionSystem
{
    public float radius = 3.0f;
    private readonly ColliderShape _shape = ColliderShape.Sphere;
    private Color _gizmoColor = Color.red;
    public ColliderShape Shape => _shape;
    public ColliderSizeData Size => new ColliderSizeData { radius = radius };
    
    public Color GizmoColor
    {
        get => _gizmoColor;
        set => _gizmoColor = value;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
