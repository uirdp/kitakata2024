using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;
using SakeShooter;


namespace SakeShooterSystems
{
    public class CollisionDetectionSystem : MonoBehaviour
    {
        [Tooltip("一度目の衝突から二回目までの間隔(秒)")]
        public float collisionDetectionInterval = 1.0f;
        public bool morePreciseCollisionDetection = false;
        
        //リストじゃなくて、連結キューを使った方がいい？
        public GameObject Masu;
        private List<ICollider> _masuColliders = new List<ICollider>();
        private List<ICollider> _bulletColldiers = new List<ICollider>();
        
        //衝突したコライダーを保存するリスト
        private HashSet<ICollider> _collidersWithDetectedCollision = new HashSet<ICollider>();

        private void Start()
        {
            ICollider MasCollider = Masu.GetComponent<ICollider>();
            AddColliderToList(MasCollider);
        }
        private void Update()
        {
            ScanColliders();
        }

        public int AddColliderToList(ICollider col)
        {
            if (col.Shape == ColliderShape.Sphere)
            {
                _bulletColldiers.Add(col);
                return _bulletColldiers.Count - 1;
            }
            else if (col.Shape == ColliderShape.Box)
            {
                _masuColliders.Add(col);
                return _masuColliders.Count - 1;
            }

            return -1;
        }
        
        public void RemoveBulletCollider(ICollider col)
        {
            _bulletColldiers.Remove(col);
        }

        public void RemoveMasuCollider(ICollider col)
        {
            _masuColliders.Remove(col);
        }

        private async UniTaskVoid RemoveFromDetectedCollisionsAfterDelay(ICollider col)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(collisionDetectionInterval));
            _collidersWithDetectedCollision.Remove(col);
        }

        private void ScanColliders()
        {
            for (int i = _masuColliders.Count - 1; i >= 0; i--)
            {
                ICollider mcol = _masuColliders[i];
                GameObject mgo = mcol.GameObject;

                //Nullならコライダーのリストから削除
                if (ReferenceEquals(mgo, null))
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
                    if (ReferenceEquals(bcol, null))
                    {
                        _bulletColldiers.RemoveAt(j);
                        continue;
                    }
                    
                    GameObject bgo = bcol?.GameObject;
                    
                    if (ReferenceEquals(bgo, null))
                    {
                        _bulletColldiers.RemoveAt(j);
                        continue;
                    }

                    
                    
                    //どちらのオブジェクトもアクティブであれば、衝突判定を行う
                    if (bgo.activeInHierarchy && CheckCollision(mcol, bcol))
                    {
                        if(!_collidersWithDetectedCollision.Contains(bcol))
                        {
                            Debug.Log($"Collision detected between {mcol.GameObject.name} and {bcol.GameObject.name}");
							//is this a callback hell?
							// ここらへんもっとわかりやすくしたい
							mcol.InvokeOnHitDetected();
                            bcol.InvokeOnHitDetected();
                            //Hitした判定をつける
                            _collidersWithDetectedCollision.Add(bcol);
                            //一定時間後にHit判定を解除
                            RemoveFromDetectedCollisionsAfterDelay(bcol).Forget();
                        }
                    }
                }
            }
        }

        private bool CheckCollision(ICollider mcol, ICollider bcol)
        {
            Assert.IsTrue(mcol.Shape == ColliderShape.Box, "mcol shape is not Box");
            Assert.IsTrue(bcol.Shape == ColliderShape.Sphere, "bcol shape is not Sphere");
            
            ColliderSizeData msize = mcol.Size;
            ColliderSizeData bsize = bcol.Size;
            
			Vector3 mpos = mcol.GameObject.transform.position;
            Vector3 bpos = bcol.GameObject.transform.position;

			//升の方向に半径の大きさだけ移動、つまり弾の中心で判定する代わりに、弾の円周上の一点で判定するすようにする。
            //大して変わらないのでoffで問題ないと思う
			if(morePreciseCollisionDetection) bpos += Vector3.Normalize(mpos-bpos) * bcol.Size.radius;

			//座標系の原点を升の中心に移動し、SDFを用いて衝突判定。
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
