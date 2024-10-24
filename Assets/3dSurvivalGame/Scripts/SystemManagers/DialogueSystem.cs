using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; set; }

        public TextMeshProUGUI dialogueText;

        public Button option1BTN;
        public Button option2BTN;

        public Canvas dialogueUI;

        public bool dialogueUIActive;

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

        // UI setactive

        public void OpenDialogueUI()
        {
            dialogueUI.gameObject.SetActive(true);
            dialogueUIActive = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void CloseDialogueUI()
        {
            dialogueUI.gameObject.SetActive(false);
            dialogueUIActive = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }



    }
}
