using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SUR
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        // -- is this item trashable --
        public bool isTrashable;

        // -- item info ui --  //public ���� �ص� ������Ʈ�� ������ �� ����, �ͱ�
        private GameObject itemInfoUI;

        private Text itemInfoUI_itemName;
        private Text itemInfoUI_itemDescription;
        private Text itemInfoUI_itemFunctionality;

        public string thisName, thisDescription, thisFunctionality;

        // -- consumption -- �Һ�
        private GameObject itemPendingConsumption;
        public bool isConsumable;

        public float healthEffect;
        public float caloriesEffect;
        public float hydrationEffect;

        // -- equipping -- 
        public bool isEquippable;
        private GameObject itemPendingEquipping;
        public bool isInsideQuickSlot;  // it's inside the slot

        public bool isSelected;     // it's the item that we selected(after we put it in the equipSlot)

        // -- usable --
        public bool isUsable;
        public GameObject itemPendingToBeUsed;

        private void Start()
        {
            itemInfoUI = InventorySystem.Instance.ItemInfoUI;
            itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
            itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent <Text>();
            itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
        }

        private void Update()
        {
            // �������� equipSlot ���� ���õǾ�������, �κ��丮�� �ű�� ���� �Ұ����ϰԲ� 
            if (isSelected)
            {
                // ������Ʈ�� ������ DragDrop ��ũ��Ʈ�� ��Ȱ��ȭ
                gameObject.GetComponent<DragDrop>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<DragDrop>().enabled = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            itemInfoUI.SetActive(true);
            itemInfoUI_itemName.text = thisName;
            itemInfoUI_itemDescription.text = thisDescription;
            itemInfoUI_itemFunctionality.text = thisFunctionality;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemInfoUI.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)   // Ŭ���� ����
        {
            //Right Mouse Button Click on
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (isConsumable)
                {
                    // Setting this specific gameobject to be the item we want to destroy later
                    itemPendingConsumption = gameObject;  // ���� ���õ� �������� ���� ���������� ����
                    consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
                }
                if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
                {
                    EquipSystem.Instance.AddToQuickSlots(gameObject);
                    isInsideQuickSlot = true;
                }
                if(isUsable)
                {
                    itemPendingToBeUsed = gameObject;

                    UseItem();
                }

            }

        }

        private void UseItem()
        {
            itemInfoUI.SetActive(false);

            // �����ִ� ��� UI �ݱ�
            InventorySystem.Instance.isOpen = false;
            InventorySystem.Instance.inventoryScreenUI.SetActive(false);

            CraftingSystem.Instance.isOpen = false;
            CraftingSystem.Instance.craftingScreenUI.SetActive(false);
            CraftingSystem.Instance.toolScreenUI.SetActive(false);
            CraftingSystem.Instance.survivalScreenUI.SetActive(false);
            CraftingSystem.Instance.refineScreenUI.SetActive(false);
            CraftingSystem.Instance.constructionScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.enabled = true;

            switch (gameObject.name)
            {
                case "Foundation(Clone)":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationMode1");
                    break;
                case "Foundation":
                    ConstructionManager.Instance.ActivateConstructionPlacement("FoundationMode1"); //for testing
                    break;
                /*case "Wall":
                    ConstructionManager.Instance.ActivateConstructionPlacement("WallModel"); 
                    break;*/
                default:
                    // do nothing
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)  // Ŭ�� ��ư up �� ����
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (isConsumable && itemPendingConsumption == gameObject)  // onPointerDown ���� ���Ҵ� ���ǰ� ��ġ�Ѵٸ�
                {
                    DestroyImmediate(gameObject);  // �׳� destroy �� �ϸ� ���� �����ӿ� �ٷ� ������, �׷��� RecalculateList�� RefreshNeededItems�� ����� �� �ٸ� ������ �� ����
                    InventorySystem.Instance.ReCalculateList();
                    CraftingSystem.Instance.RefreshNeededItems();
                }
                if (isUsable && itemPendingToBeUsed == gameObject)
                {
                    DestroyImmediate(gameObject);                           //
                    InventorySystem.Instance.ReCalculateList();             //
                    CraftingSystem.Instance.RefreshNeededItems();           // �κ��丮 ���ǿ� ��ȭ�� ������ �׻� �� �ڵ� 3���� ����ϴµ�
                }
            }
        }

        private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
        {
            itemInfoUI.SetActive(false);   // �������� ����ϸ� ����â�� ���̻� ��Ÿ���� �ȵǱ� ����

            healthEffectCalculation(healthEffect);

            caloriesEffectCalculation(caloriesEffect);

            hydrationEffectCalculation(hydrationEffect);

        }


        private static void healthEffectCalculation(float healthEffect)
        {
            // --- Health --- //

            float healthBeforeConsumption = PlayerState.Instance.currentHealth;
            float maxHealth = PlayerState.Instance.maxHealth;

            if (healthEffect != 0)  // there is some kind of healthEffect
            {
                if ((healthBeforeConsumption + healthEffect) > maxHealth)  // ������� + ������ȿ���� maxHealth ���� Ŭ ���
                {
                    PlayerState.Instance.setHealth(maxHealth);
                }
                else
                {
                    PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
                }
            }
        }


        private static void caloriesEffectCalculation(float caloriesEffect)
        {
            // --- Calories --- //

            float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
            float maxCalories = PlayerState.Instance.maxCalories;

            if (caloriesEffect != 0)
            {
                if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
                {
                    PlayerState.Instance.setCalories(maxCalories);
                }
                else
                {
                    PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
                }
            }
        }
        private static void hydrationEffectCalculation(float hydrationEffect)
        {
            // --- Hydration --- //

            float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
            float maxHydration = PlayerState.Instance.maxHydrationPercent;

            if (hydrationEffect != 0)
            {
                if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
                {
                    PlayerState.Instance.setHydration(maxHydration);
                }
                else
                {
                    PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
                }
            }
        }
    }
}

