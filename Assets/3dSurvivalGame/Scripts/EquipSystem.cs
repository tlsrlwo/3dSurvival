using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class EquipSystem : MonoBehaviour
    {
        public static EquipSystem Instance { get; set; }

        [Header("UI")]
        public GameObject quickSlotPanel;

        public List<GameObject> quickSlotsList = new List<GameObject>();
        public List<string> itemList = new List<string>();

        [Header("NumbersHolder")]
        public GameObject numbersHolder;

        public int selectedNumber = -1;
        public GameObject selectedItem;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            PopulateSlotList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectQuickSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectQuickSlot(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectQuickSlot(3);
            }
            else if (!Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectQuickSlot(4);
            }
            else if (!Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectQuickSlot(5);
            }
            else if (!Input.GetKeyDown(KeyCode.Alpha6))
            {
                SelectQuickSlot(6);
            }
            else if (!Input.GetKeyDown(KeyCode.Alpha7))
            {
                SelectQuickSlot(7);
            }
        }

        void SelectQuickSlot(int number)
        {
            if (checkIfSlotIsFull(number) == true)
            {
               if(selectedNumber != number)  // 기존 선택된 번호가 아닐 시
                {
                    selectedNumber = number;

                    // unselect previously selectedItem
                    if (selectedItem != null)  // something else is selected
                    {
                        selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    }

                    selectedItem = GetSelectedItem(number);
                    selectedItem.GetComponent<InventoryItem>().isSelected = true;

                    // changing the colour
                    foreach (Transform child in numbersHolder.transform)
                    {
                        child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                    }

                    Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                    toBeChanged.color = Color.white;
                }


                // 선택된 번호를 또다시 선택함 (1번 슬롯을 1 번을 다시 누르면 비활성화 되게)
                else
                {
                    selectedNumber = -1;  // -1 은 null 이랑 같은의미 
                                          // unselect previously selectedItem
                    if (selectedItem != null)  // something else is selected
                    {
                        selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    }
                    // changing the colour
                    foreach (Transform child in numbersHolder.transform)
                    {
                        child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                    }
                }
            }
        }

        GameObject GetSelectedItem(int slotNumber)
        {
            return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject; 
        }

        bool checkIfSlotIsFull(int slotNumber)
        {
            if (quickSlotsList[slotNumber-1].transform.childCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PopulateSlotList()
        {
            foreach (Transform child in quickSlotPanel.transform)
            {
                if(child.CompareTag("QuickSlot"))
                {
                    quickSlotsList.Add(child.gameObject);
                }
            }
        }

        public void AddToQuickSlots(GameObject itemToEquip)
        {            
            GameObject availableSlot = FindNextEmptySlot();
            itemToEquip.transform.SetParent(availableSlot.transform, false);  // bool 형은 worldPos 을 위한 bool. true 로 해놓으면 위치는 그대로, 부모오브젝트만 바뀜
            string cleanName = itemToEquip.name.Replace("(Clone)", "");
            itemList.Add(cleanName);

            InventorySystem.Instance.ReCalculateList();
        }

        private GameObject FindNextEmptySlot()
        {
            foreach (GameObject slot in quickSlotsList)
            {
                if(slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
            return new GameObject();
        }

        public bool CheckIfFull()
        {
            int counter = 0;

            foreach (GameObject slot in quickSlotsList)
            {
                if(slot.transform.childCount > 0)
                {
                    counter += 1;
                }                
            }
            if( counter == 7)
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
