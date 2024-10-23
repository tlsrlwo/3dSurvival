using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class NPC : MonoBehaviour
    {

        public bool playerInRange;

        public bool isTalkingWithPlayer;

       

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }

        internal void StartConversation()
        {
            isTalkingWithPlayer = true;
            print("conversation");
        }


    }
}
