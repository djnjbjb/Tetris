using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris.Sound
{
    public class SoundEffect : MonoBehaviour
    {
        #region Singleton
        public static SoundEffect singleton;
        void SetSingleton()
        {
            singleton = this;
        }
        #endregion

        [SerializeField] AudioClip action;
        [SerializeField] AudioClip smallSuccess;
        [SerializeField] AudioClip bigSuccess;
        [SerializeField] AudioClip failure;

        AudioSource audioSource;

        void Awake()
        {
            SetSingleton();
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAction()
        {
            audioSource.PlayOneShot(action);
        }

        public void PlaySmallSuccess()
        {
            audioSource.PlayOneShot(smallSuccess);
        }

        public void PlayBigSuccess()
        {
            audioSource.PlayOneShot(bigSuccess);
        }

        public void PlayFailure()
        {
            audioSource.PlayOneShot(failure);
        }
    }
}