using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooterSystems
{
    public class CollisionDetectionSystem : MonoBehaviour
    {
        private List<ICollider> _colliders = new List<ICollider>();
        
        public void AddColliderToList(GameObject go)
        {
            ICollider col = go.GetComponent<ICollider>();
            _colliders.Add(col);
        }
        
        public void RemoveColliderFromList(ICollider col)
        {
            _colliders.Remove(col);
        }

        private void CheckCollision()
        {
            for (int i = _colliders.Count - 1; i >= 0; i--)
            {
                ICollider col = _colliders[i];
                GameObject go = col.GameObject;

                if (go.activeInHierarchy)
                {
                    Debug.Log("Collision Check");
                }
                
                else _colliders.RemoveAt(i);
            }
        }
    }
}
