using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooter
{
    public class Masu : MonoBehaviour
    {
        [SerializeField] private MasuStatus _status;
        [SerializeField] private MasuMovement _movement;
        
        public MasuStatus Status => _status;
        public MasuMovement Movement => _movement;
    }
}
