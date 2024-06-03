using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SakeShooterSystems;

public class SphereCollider : MonoBehaviour, ICollisionSystem
{
    public float radius = 3.0f;
    private readonly ColliderShape _shape = ColliderShape.Sphere;
    private Color gizmoColor = Color.red;
    public ColliderShape Shape => _shape;
    public ColliderSizeData Size => new ColliderSizeData { radius = radius };
    
    public Color GizmoColor
    {
        get => gizmoColor;
        set => gizmoColor = value;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
