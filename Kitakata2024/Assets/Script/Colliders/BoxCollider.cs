using System.Collections;
using System.Collections.Generic;
using SakeShooterSystems;
using UnityEngine;

public class BoxCollider : MonoBehaviour, ICollider
{
    public Vector3 size = new Vector3(2.0f, 2.0f, 2.0f);
    public Color gizmoColor = Color.yellow;
    
    private readonly ColliderShape _shape = ColliderShape.Box;
    
    public ColliderShape Shape => _shape;
    public bool IsEnable => enabled;
    public GameObject GameObject => gameObject;
    public ColliderSizeData Size => new ColliderSizeData { size = size };
    
    public void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
