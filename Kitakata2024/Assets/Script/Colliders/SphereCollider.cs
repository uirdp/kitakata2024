using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SakeShooterSystems;

public class SphereCollider : MonoBehaviour, ICollider
{
    public float radius = 3.0f;
    public Color gizmoColor = Color.red;
    
    private readonly ColliderShape _shape = ColliderShape.Sphere;
    
    public ColliderShape Shape => _shape;
    public bool IsEnable => enabled;
    public GameObject GameObject => gameObject;
    public ColliderSizeData Size => new ColliderSizeData { radius = radius };
    
    public void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
}
