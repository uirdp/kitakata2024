using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooterSystems
{
    #region colliders
    public enum ColliderType
    {
        Sphere,
        Box,
    }
    interface ICollisionSystem
    {
        void CheckCollision(ICollisionSystem other);
    }
    
    #endregion

}