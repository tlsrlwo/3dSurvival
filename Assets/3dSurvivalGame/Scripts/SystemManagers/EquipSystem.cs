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

        [Header("ToolHolder")]
        public GameObject ToolHolder;
        public GameObject selectedItemModel;

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
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectQuickSlot(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectQuickSlot(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SelectQuickSlot(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SelectQuickSlot(7);
            }
        }

        void SelectQuickSlot(int number)
        {
            if (checkIfSlotIsFull(number) == true) // �ȿ� ������ �־�� ������ �� �ֱ� ������ true
            {
               if(selectedNumber != number)  // ���� ���õ� ��ȣ�� �ƴ� ��
                {
                    selectedNumber = number;

                    // unselect previously selectedItem
                    if (selectedItem != null)  // something else is selected
                    {
                        selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    }

                    selectedItem = GetSelectedItem(number);
                    selectedItem.GetComponent<InventoryItem>().isSelected = true;

                    SetEquippedModel(selectedItem);

                    // changing the colour
                    foreach (Transform child in numbersHolder.transform)
                    {
                        child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                    }

                    Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                    toBeChanged.color = Color.white;
                }


                // ���õ� ��ȣ�� �Ǵٽ� ������ (1�� ������ 1 ���� �ٽ� ������ ��Ȱ��ȭ �ǰ�)
                else
                {
                    selectedNumber = -1;  // -1 �� null �̶� �����ǹ� 
                                          // unselect previously selectedItem
                    if (selectedItem != null)  // something else is selected
                    {
                        selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                        selectedItem = null;
                    }

                    if(selectedItemModel != null)
                    {
                        DestroyImmediate(selectedItemModel.gameObject);   
                        selectedItemModel = null; 
                        // destroy �� �ϸ� selectedItemModel ���� inspector ���� missing ���� ǥ�õǱ⶧���� null ������ �缳�� ���� 
                        Debug.Log("Currently Selected Model destroyed");
                    }

                    // changing the colour
                    foreach (Transform child in numbersHolder.transform)
                    {
                        child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                    }
                }
            }
        }

        private void SetEquippedModel(GameObject selectedItem)
        {
            // �̹� ���õ� �������� ������, �� �������� ����� ���� ���õ� �������� ����
            if (selectedItemModel != null)
            {
                DestroyImmediate(selectedItemModel.gameObject);
                selectedItemModel = null;
                Debug.Log("selected item deleted");
            }

            string selectedItemName = selectedItem.name.Replace("(Clone)", "");
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(0.91f, -1.55f, 1.7f), Quaternion.Euler(-7.773f, -88.4f, 0));
            selectedItemModel.transform.SetParent(ToolHolder.transform, false);
            Debug.Log("item instantiated");
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
            itemToEquip.transform.SetParent(availableSlot.transform, false);  // bool ���� worldPos �� ���� bool. true �� �س����� ��ġ�� �״��, �θ������Ʈ�� �ٲ�
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
