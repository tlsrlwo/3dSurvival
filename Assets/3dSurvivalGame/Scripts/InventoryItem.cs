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

        // -- item info ui --  //public 으로 해도 오브젝트를 가져올 수 없음, 왤까
        private GameObject itemInfoUI;

        private Text itemInfoUI_itemName;
        private Text itemInfoUI_itemDescription;
        private Text itemInfoUI_itemFunctionality;

        public string thisName, thisDescription, thisFunctionality;

        // -- consumption -- 소비
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
            // 아이템이 equipSlot 에서 선택되어있으면, 인벤토리로 옮기는 동작 불가능하게끔 
            if (isSelected)
            {
                // 컴포넌트로 부착된 DragDrop 스크립트를 비활성화
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

        public void OnPointerDown(PointerEventData eventData)   // 클릭시 실행
        {
            //Right Mouse Button Click on
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (isConsumable)
                {
                    // Setting this specific gameobject to be the item we want to destroy later
                    itemPendingConsumption = gameObject;  // 지금 선택된 아이템이 사용될 아이템임을 지정
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

            // 열려있는 모든 UI 닫기
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

        public void OnPointerUp(PointerEventData eventData)  // 클릭 버튼 up 시 실행
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (isConsumable && itemPendingConsumption == gameObject)  // onPointerDown 에서 돌았던 조건과 일치한다면
                {
                    DestroyImmediate(gameObject);  // 그냥 destroy 를 하면 다음 프레임에 바로 삭제됨, 그러면 RecalculateList와 RefreshNeededItems가 실행될 때 다른 상태일 수 있음
                    InventorySystem.Instance.ReCalculateList();
                    CraftingSystem.Instance.RefreshNeededItems();
                }
                if (isUsable && itemPendingToBeUsed == gameObject)
                {
                    DestroyImmediate(gameObject);                           //
                    InventorySystem.Instance.ReCalculateList();             //
                    CraftingSystem.Instance.RefreshNeededItems();           // 인벤토리 물건에 변화가 있으면 항상 이 코드 3줄을 사용하는듯
                }
            }
        }

        private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
        {
            itemInfoUI.SetActive(false);   // 아이템을 사용하면 설명창이 더이상 나타나면 안되기 때문

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
                if ((healthBeforeConsumption + healthEffect) > maxHealth)  // 현재상태 + 아이템효과가 maxHealth 보다 클 경우
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

