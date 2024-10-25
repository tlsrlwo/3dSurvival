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

        // used in saveManager for pickedUpItems
        public List<string> itemsPickedUp;


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

            Cursor.visible = false;
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
            if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
            {

                Debug.Log("i is pressed");

                inventoryScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.I) && isOpen)
            {
                inventoryScreenUI.SetActive(false);
                if (!CraftingSystem.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }
                isOpen = false;
            }
        }

        public void AddToInventory(string itemName)
        {
            if (SaveManager.Instance.isLoading == false)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.pickUpItemSound);
            }

            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);

            TriggerPickupPopup(itemName, sprite);
            StartCoroutine(PickUpPopupDisappear());

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();

            QuestManager.Instance.RefreshTrackerList();

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
        public bool CheckSlotsAvailable(int emptyNeeded)        // 필요한 여유 칸이 몇개인지 
        {
            int emptySlot = 0;

            foreach (GameObject slot in slotList)
            {
                if(slot.transform.childCount <= 0)
                {
                    emptySlot += 1;
                }
            }
                if(emptySlot >= emptyNeeded)                  
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

            // 인벤토리의 아이템을 뒤에서부터 지우기 위해, 3개의 아이템을 지워야하면 3번 반복문을 돌아가게 하기 위해 for문 사용
            for(var i = slotList.Count - 1; i >= 0; i--)
            {
                // childCount > 0 은 슬롯에 아이템이 있다는 뜻 (즉 1개 > 0)
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

            QuestManager.Instance.RefreshTrackerList();
        }

        public void ReCalculateList()
        {
            itemList.Clear();

            foreach(GameObject slot in slotList)
            {
                // itemList(string형식)을 클리어하고, 슬롯을 검사해서 아이템이 있다면 슬롯의 아이템 뒤 (Clone) 을 제거한 뒤 itemList(string) 에 다시 추가
                if(slot.transform.childCount > 0)
                {
                    string name = slot.transform.GetChild(0).name;      // item (Clone)                                        
                    string str2 = "(Clone)";
                    string result = name.Replace(str2, "");             // replace the "(Clone)" to " " NULL, so only the 'item' name is left

                    itemList.Add(result);
                }
            }
        }

        // QeustManager TrackerUI 에서 사용됨
        public int CheckItemAmount(string name)
        {
            int itemCounter = 0;

            foreach(string item in itemList)
            {
                if(item == name)
                {
                    itemCounter++;
                }
            }
            return itemCounter;
        }


    }
}