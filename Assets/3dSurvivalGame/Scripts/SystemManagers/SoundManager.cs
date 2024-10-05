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

        // ���� PlayDropItemSound ���µ� �׷��� �� �Ҹ��� ���� �Լ��� ��������� �ż�, �ϳ��� �Լ��� ���� �Ҹ��� �ҷ��� �� �ְ� PlaySound(AudioSource soundToPlay)�� �ٲ�
        public void PlaySound(AudioSource soundToPlay)
        {
            if(!soundToPlay.isPlaying)
            {
                soundToPlay.Play();
            }
        }

    }
}
