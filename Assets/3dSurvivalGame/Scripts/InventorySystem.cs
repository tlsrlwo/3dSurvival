using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{

    public class InventorySystem : MonoBehaviour
    {

        public static InventorySystem Instance { get; set; }

        public GameObject inventoryScreenUI;

        public List<GameObject> slotList = new List<GameObject>();  //contains the slot

        public List<string> itemList = new List<string>();          //contain item names in the slot

        private GameObject itemToAdd;
        private GameObject whatSlotToEquip;

        public bool isOpen;
        //public bool isFull;


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


        void Start()
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
            

            PopulateSlotList();
        }

        private void PopulateSlotList()
        {
            foreach(Transform child in inventoryScreenUI.transform) // uses transform to find the child objects
            {
                if(child.CompareTag("Slot"))                        // later the canvas will be added buttons or else, so only finds objects tagged 'slot'
                {
                    slotList.Add(child.gameObject);
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && !isOpen)
            {

                Debug.Log("i is pressed");
    
                inventoryScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.I) && isOpen)
            {
                inventoryScreenUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
        }

        public void AddToInventory(string itemName)         
        {            
                whatSlotToEquip = FindNextEmptySlot();
                               
                itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
                itemToAdd.transform.SetParent(whatSlotToEquip.transform);

                itemList.Add(itemName);            
        }

        
        private GameObject FindNextEmptySlot()
        {
            foreach(GameObject slot in slotList)
            {
                if(slot.transform.childCount == 0)
                {
                    return slot;
                }
            }

            return new GameObject();
        }

        // function that checks inventory status
        public bool CheckIfFull()
        {
            int counter = 0;

            foreach (GameObject slot in slotList)
            {
                if(slot.transform.childCount > 0)
                {
                    counter += 1;
                }
            }
                if(counter == 21)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
}