using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; set; }

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



    }
}
