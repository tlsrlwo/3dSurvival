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

        bool isOpen;

        // All Blueprint


        public static CraftingSystem Instance { get; set; }

        private void Awake()
        {
            if(Instance == null && Instance != this)
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
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
