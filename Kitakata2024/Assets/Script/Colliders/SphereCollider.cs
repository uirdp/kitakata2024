using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SakeShooterSystems;

public class SphereCollider : MonoBehaviour, ICollisionSystem
{
    public float radius = 3.0f;
    private ColliderShape _shape = ColliderShape.Sphere;
    public ColliderShape Shape => _shape;
    public ColliderSizeData Size => new ColliderSizeData { radius = radius };
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
