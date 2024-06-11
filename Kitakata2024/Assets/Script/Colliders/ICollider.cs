using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace SakeShooterSystems
{
    #region colliders
    public enum ColliderShape
    {
        Sphere,
        Box,
    }
    //Union構造体
    [System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
    public struct ColliderSizeData
    {
        [FieldOffset(0)] public float radius;
        [FieldOffset(0)] public Vector3 size;
    }
    
    public interface ICollider
    {
        ColliderShape Shape { get; }
        ColliderSizeData Size { get; }
        GameObject GameObject { get; }
        bool IsEnable { get; }
        
        void OnDrawGizmos();
        void RegisterOnHitDetected(Action action);
        void UnregisterOnHitDetected();
    }
    
    #endregion

}