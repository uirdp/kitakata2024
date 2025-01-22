using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SakeShooterSystems{
    // SoundManagerに変更かな？
    public class SoundEffectManager : MonoBehaviour
    {
        
        [Header("-------- Background Music ----------")]
        public AudioClip restMusic;
        public AudioClip gameMusic;
        
        [Header("-------- Sound Effects ----------")]
        public AudioClip successSound;
        public AudioClip failureSound;
        public AudioClip pauseSound;
        public AudioClip resumeSound;
        public AudioClip gameStartSound;
        public AudioClip gameEndSound;
        
        
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 0.5f;
        }
        
        public void PlayRestMusic()
        {
            _audioSource.clip = restMusic;
            _audioSource.Play();
        }
        
        public void PlayGameMusic()
        {
            _audioSource.clip = gameMusic;
            _audioSource.Play();
        }
        public void PlaySuccessSound()
        {
            _audioSource.PlayOneShot(successSound);
        }
        
        public void PlayFailureSound()
        {
            _audioSource.PlayOneShot(failureSound);
        }
        
        public void PlayPauseSound()
        {
            _audioSource.PlayOneShot(pauseSound);
        }
        
        public void PlayResumeSound()
        {
            _audioSource.PlayOneShot(resumeSound);
        }
        
        public void PlayGameStartSound()
        {
            _audioSource.PlayOneShot(gameStartSound);
        }
        
        public void PlayGameEndSound()
        {
            _audioSource.PlayOneShot(gameEndSound);
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