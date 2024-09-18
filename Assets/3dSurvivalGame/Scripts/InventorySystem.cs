using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{

    public class InventorySystem : MonoBehaviour
    {
        // info_UI
        public GameObject ItemInfoUI;


        public static InventorySystem Instance { get; set; }

        public GameObject inventoryScreenUI;

        public List<GameObject> slotList = new List<GameObject>();  //contains the slot

        public List<string> itemList = new List<string>();          //contain item names in the slot

        private GameObject itemToAdd;
        private GameObject whatSlotToEquip;

        public bool isOpen;
        public bool isFull;

        // PickUpPopUp
        public GameObject pickupAlert;
        public Text pickupName;
        public Image pickupImage;
        public float pickupAlertDisappearTime = 1;


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
                if (!CraftingSystem.Instance.isOpen)
                {
                Cursor.lockState = CursorLockMode.Locked;
                }
                isOpen = false;
            }
        }

        public void AddToInventory(string itemName)
        {
            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);

            TriggerPickupPopup(itemName, sprite);
            StartCoroutine(PickUpPopupDisappear());

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();

        }

        public void TriggerPickupPopup(string itemName, Sprite itemSprite)
        {
            pickupAlert.SetActive(true);

            pickupName.text = itemName + " Added To Inventory";
            pickupImage.sprite = itemSprite;
        }

        public IEnumerator PickUpPopupDisappear()
        {
            yield return new WaitForSeconds(pickupAlertDisappearTime);
            pickupAlert.SetActive(false);
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

        // after crafting, remove item from the inventory
        public void RemoveItem(string nameToRemove, int amountToRemove)
        {
            int counter = amountToRemove;

            // �κ��丮�� �������� �ڿ������� ����� ����, 3���� �������� �������ϸ� 3�� �ݺ����� ���ư��� �ϱ� ���� for�� ���
            for(var i = slotList.Count - 1; i >= 0; i--)
            {
                // childCount > 0 �� ���Կ� �������� �ִٴ� �� (�� 1�� > 0)
                if (slotList[i].transform.childCount > 0)
                {
                    if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                    {
                        DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                        counter -= 1;
                    }
                }
            }
            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }

        public void ReCalculateList()
        {
            itemList.Clear();

            foreach(GameObject slot in slotList)
            {
                // itemList(string����)�� Ŭ�����ϰ�, ������ �˻��ؼ� �������� �ִٸ� ������ ������ �� (Clone) �� ������ �� itemList(string) �� �ٽ� �߰�
                if(slot.transform.childCount > 0)
                {
                    string name = slot.transform.GetChild(0).name;      // item (Clone)                                        
                    string str2 = "(Clone)";
                    string result = name.Replace(str2, "");             // replace the "(Clone)" to " " NULL, so only the 'item' name is left

                    itemList.Add(result);
                }
            }
        }
    }
}