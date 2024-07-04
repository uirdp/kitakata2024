using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooterSystems{
    public class SoundEffectManager : MonoBehaviour
    {
        public AudioClip successSound;
        public AudioClip failureSound;
        
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 0.02f;
        }
        
        private void PlaySuccessSound()
        {
            _audioSource.PlayOneShot(successSound);
        }
        
        private void PlayFailureSound()
        {
            _audioSource.PlayOneShot(failureSound);
        }
        
        public void PlayExitSound(MasuExitStatus status)
        {
            switch (status)
            {
                case MasuExitStatus.Success:
                    PlaySuccessSound();
                    break;
                case MasuExitStatus.Failure:
                    PlayFailureSound();
                    break;
            }
        }
    }

}