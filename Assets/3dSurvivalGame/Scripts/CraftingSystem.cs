using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class CraftingSystem : MonoBehaviour
    {
        public GameObject craftingScreenUI;
        public GameObject toolScreenUI;

        public List<string> inventoryItemList = new List<string>();

        // Category Button
        Button toolsBTN;

        // Craft Button
        Button craftAxeBTN;

        // Requirement Text
        Text AxeReq1, AxeReq2;

        public bool isOpen;

        // All Blueprint


        public static CraftingSystem Instance { get; set; }

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

        // Start is called before the first frame update
        void Start()
        {
            isOpen = false;

            toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
            toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

            // Axe
            AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
            AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

            craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent <Button>();
            craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(); });
        }

        void OpenToolsCategory()
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(true);
        }

        void CraftAnyItem()
        {
            // add item into inventory


            // remove resources from intentory (b/c its used)





        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) && !isOpen)
            {

                Debug.Log("C is pressed");

                craftingScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.C) && isOpen)
            {
                craftingScreenUI.SetActive(false);
                toolScreenUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }

        }
    }
}
