using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class MasuParticle : MonoBehaviour
    {
        public ParticleSystem ps;

        private void Start()
        {
            ps.transform.position = this.transform.position;
        }
    }
}
