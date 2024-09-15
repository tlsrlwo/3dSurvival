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

        // Interact ������ �������̸� �������� �ٲ�� �κ�
        public Image centerDotImage;
        public Image handIcon;


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
                    }
                    else
                    {
                        handIcon.gameObject.SetActive(false);
                        centerDotImage.gameObject.SetActive(true);
                    }


                }
                else // ray �� �繰�� �ε������� InteractableObjects �� ���� ���
                {
                    onTarget= false;
                    interaction_Info_UI.SetActive(false);
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                }

            }
            else // ray �� �ε����� �ʾ��� ���
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
            }
        }
    }
}