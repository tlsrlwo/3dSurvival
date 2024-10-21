using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class LoadSlot : MonoBehaviour
    {
        private Button button;
        private TextMeshProUGUI buttonText;

        public int slotNumber;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }
           
        private void Update()
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                buttonText.text = "";
            }
            else
            {
                buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
            }
        }

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                if(SaveManager.Instance.IsSlotEmpty(slotNumber) == false)
                {
                    SaveManager.Instance.LoadGameWhenGameStarts(slotNumber);
                    SaveManager.Instance.DeselectButton();
                }
                else
                {
                    // if empty do nothing;
                }
            }
            );
        }

    }
}
