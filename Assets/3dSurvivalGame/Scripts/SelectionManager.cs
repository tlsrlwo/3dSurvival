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


        public GameObject interaction_Info_UI;
        TextMeshProUGUI interaction_text;       

        public bool onTarget;

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

                    interaction_text.text = Interactable.GetItemName();
                    interaction_Info_UI.SetActive(true);
                }
                else // ray 가 사물에 부딪혔지만 InteractableObjects 가 없는 경우
                {
                    onTarget= false;
                    interaction_Info_UI.SetActive(false);                       
                }

            }
            else // ray 가 부딪히지 않았을 경우
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
        }
    }
}