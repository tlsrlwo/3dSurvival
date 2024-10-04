using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {


        public static SoundManager Instance { get; set; }

        public AudioSource dropItemsound;
        public AudioSource grassWalkSound;
        public AudioSource chopSound;






        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void PlayDropsound()
        {
            if(!dropItemsound.isPlaying)
            {
                dropItemsound.Play();
            }
        }
    }
}
