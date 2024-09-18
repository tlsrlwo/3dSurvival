using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SUR
{
    public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject trashAlertUI;

        private Text textToModify;
        public Sprite trash_closed;
        public Sprite trash_opened;

        private Image imageComponent;

        Button yesBTN, noBTN; 
        
        // itemBeingDragged 가 static GameObject 이고 가져올 때 마다 바뀌기 때문에 start 에서 지정할 수 없음. 그래서 get 형식을 써서 가져옴
        GameObject draggedItem
        {
            get
            {
                return DragDrop.itemBeingDragged;
            }
        }

        GameObject itemToBeDeleted;

        public string itemName
        {
            get
            {
                string name = itemToBeDeleted.name;
                string toRemove = "(Clone)";
                string result = name.Replace(toRemove, "");
                return result;
            }
        }

        private void Start()
        {
            imageComponent = transform.Find("trashcan").GetComponent<Image>();

            textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();
            
            yesBTN = trashAlertUI.transform.Find("Yes").GetComponent<Button>();
            yesBTN.onClick.AddListener(delegate { DeleteItem(); });

            noBTN = trashAlertUI.transform.Find("No").GetComponent<Button>();
            noBTN.onClick.AddListener(delegate { CancelDeletion(); });
        }

        public void OnDrop(PointerEventData eventData)  //IDropHandler
        {
            if(draggedItem.GetComponent<InventoryItem>().isTrashable == true)
            {
                itemToBeDeleted = draggedItem.gameObject;

                StartCoroutine(notifyBeforeDeletion());
            }
        }

        IEnumerator notifyBeforeDeletion()
        {
            trashAlertUI.SetActive(true);
            textToModify.text = "Throw away this " + itemName + "?";
            yield return new WaitForSeconds(1f);
        }

        private void CancelDeletion()
        {
            imageComponent.sprite = trash_closed;
            trashAlertUI.SetActive(false);
        }

        private void DeleteItem()
        {
            imageComponent.sprite = trash_opened;
            DestroyImmediate(itemToBeDeleted.gameObject);
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            trashAlertUI.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)  // 아이템을 trash slot 에 갖다대면은 trashslot sprite 가 바뀜
        {
            if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
            {
                imageComponent.sprite = trash_opened;
            }        
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
            {
                imageComponent.sprite = trash_closed;
            }
        }
    }
}
