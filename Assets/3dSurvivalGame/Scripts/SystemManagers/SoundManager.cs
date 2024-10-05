using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        // Sound Effects
        public static SoundManager Instance { get; set; }

        public AudioSource dropItemsound;

        public AudioSource craftingSound;
        public AudioSource toolSwingSound;
        public AudioSource chopSound;
        public AudioSource pickUpItemSound;

        public AudioSource grassWalkSound;

        // Music
        public AudioSource startingZoneBGMusic;

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

        // 원래 PlayDropItemSound 였는데 그러면 각 소리에 각각 함수를 구현해줘야 돼서, 하나의 함수로 많은 소리를 불러올 수 있게 PlaySound(AudioSource soundToPlay)로 바꿈
        public void PlaySound(AudioSource soundToPlay)
        {
            if(!soundToPlay.isPlaying)
            {
                soundToPlay.Play();
            }
        }

    }
}
