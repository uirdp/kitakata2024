using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SakeShooterSystems
{
    public class CollisionDetectionSystem : MonoBehaviour
    {
        //リストじゃなくて、連結キューを使った方がいい？
        //升には連結キューを用いるが、弾にはリストを用いる（プールを使っているため、挙動がわからない）
        [SerializeField]
        private List<ICollider> _masuColliders = new List<ICollider>();
        private List<ICollider> _bulletColldiers = new List<ICollider>();
        
        private void Update()
        {
            ScanColliders();
        }

        public void AddColliderToList(GameObject go)
        {
            ICollider col = go.GetComponent<ICollider>();
            _masuColliders.Add(col);
        }

        public void RemoveColliderFromList(ICollider col)
        {
            _masuColliders.Remove(col);
        }

        private void ScanColliders()
        {
            for (int i = _masuColliders.Count - 1; i >= 0; i--)
            {
                ICollider mcol = _masuColliders[i];
                GameObject mgo = mcol.GameObject;

                //Nullならコライダーのリストから削除
                if (mgo == null)
                {
                    _masuColliders.RemoveAt(i);
                    continue;
                }

                //もしもオブジェクトが非アクティブならスキップ
                if (!mgo.activeInHierarchy)
                    continue;

                for (int j = _bulletColldiers.Count - 1; j >= 0; j--)
                {
                    ICollider bcol = _bulletColldiers[j];
                    GameObject bgo = bcol.GameObject;

                    if (bgo == null)
                    {
                        _bulletColldiers.RemoveAt(j);
                        continue;
                    }
                    
                    //どちらのオブジェクトもアクティブであれば、衝突判定を行う
                    if (bgo.activeInHierarchy && CheckCollision(mcol, mcol))
                    {
                        Debug.Log("Collision Detected!");
                    }
                }
            }
        }

        private bool CheckCollision(ICollider mcol, ICollider bcol)
        {
            Assert.IsTrue(mcol.Shape == ColliderShape.Box, "mcol shape is not Box");
            Assert.IsTrue(bcol.Shape == ColliderShape.Sphere, "bcol shape is not Sphere");
            
            ColliderSizeData msize = mcol.Size;
            
            Vector3 mpos = mcol.GameObject.transform.position;
            Vector3 bpos = bcol.GameObject.transform.position;
            
            //座標系の原点を升の中心に移動し、SDFを用いて衝突判定
            float d = SdBox(bpos - mpos, msize.size);
            return d < 0;
        }
        
        #region sdf
        //https://iquilezles.org/articles/distfunctions/
        private float SdBox(Vector3 p, Vector3 b)
        {
            Vector3 q = Abs(p) - b;
            return Max(q, Vector3.zero).magnitude + Mathf.Min(Mathf.Max(q.x, Mathf.Max(q.y, q.z)), 0.0f);
        }

        private Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        private Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
        }
        
        #endregion
    }
}
