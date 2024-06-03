using System.Runtime.InteropServices;
using Unity.VisualScripting;
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
    public interface ICollisionSystem
    {
        //Check collision　は別の監視クラスにつけよう
        ColliderShape Shape { get; }
        ColliderSizeData Size { get; }
        
        void OnDrawGizmos(Color color);
    }
    
    #endregion

}