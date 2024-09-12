using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class InteractableObjects : MonoBehaviour
    {
        public string ItemName;
        public bool playerInRange;

        public string GetItemName()
        {
            return ItemName;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget)
            {
                Debug.Log("Item added to Inventory");

                Destroy(this.gameObject);
            }
            

        }
        private void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag("Player"))
            {
                playerInRange = true;


            }
        }

        private void OnTriggerExit(Collider col)
        {
            if(col.CompareTag("Player"))
            {
                playerInRange = false;


            }
        }

    }
}