using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; set; }

        public bool onTarget;

        public GameObject selectedObject;

        public GameObject interaction_Info_UI;
        TextMeshProUGUI interaction_text;

        // Interact 가능한 아이템이면 조준점이 바뀌는 부분
        public Image centerDotImage;
        public Image handIcon;

        public bool handIsVisible;

        public GameObject selectedTree;
        public GameObject chopHolder;


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

        private void Start()
        {
            interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                var selectionTransform = hit.transform;

                InteractableObjects Interactable = selectionTransform.GetComponent<InteractableObjects>();

                ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

                if(choppableTree && choppableTree.playerInRange)   //if choppableTree is not null and playerInRange is true
                {
                    choppableTree.canBeChopped = true;
                    selectedTree = choppableTree.gameObject;
                    chopHolder.gameObject.SetActive(true);

                }
                else
                {
                    if (selectedTree != null)
                    {
                        selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                        selectedTree = null;
                        chopHolder.gameObject.SetActive (false);
                    }
                }


                if (Interactable && Interactable.playerInRange) 
                {
                    onTarget = true;
                    selectedObject = Interactable.gameObject; 
                    interaction_text.text = Interactable.GetItemName();
                    interaction_Info_UI.SetActive(true);

                    if(Interactable.CompareTag("Pickable") )
                    {
                        centerDotImage.gameObject.SetActive(false);
                        handIcon.gameObject.SetActive(true);

                        handIsVisible = true;
                    }
                    else
                    {
                        handIcon.gameObject.SetActive(false);
                        centerDotImage.gameObject.SetActive(true);

                        handIsVisible = false;
                    }


                }
                else // ray 가 사물에 부딪혔지만 InteractableObjects 가 없는 경우
                {
                    onTarget= false;
                    interaction_Info_UI.SetActive(false);
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);

                    handIsVisible = false;
                }

            }
            else // ray 가 부딪히지 않았을 경우
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                centerDotImage.gameObject.SetActive(true);

                handIsVisible = false;
            }
        }

        public void DisableSelection()
        {
            handIcon.enabled = false;
            centerDotImage.enabled = false;
            interaction_Info_UI.SetActive(false);

            selectedObject = null;
        }

        public void EnableSelection()
        {
            handIcon.enabled = true;
            centerDotImage.enabled = true;
            interaction_Info_UI.SetActive(true);
        }
    }
}