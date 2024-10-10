using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class CraftingSystem : MonoBehaviour
    {
        public GameObject craftingScreenUI;
        public GameObject toolScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

        public List<string> inventoryItemList = new List<string>();

        //--------------------------------------------------------------CategoryButton(CraftingUI)
        #region ("CategoryButton(CraftingUI)")    
        Button toolsBTN,
            survivalBTN,
            refineBTN,
            constructionBTN;


        #endregion

        //--------------------------------------------------------------CraftButtons
        #region ("CraftButtons")
        Button craftAxeBTN,
            craftPlankBTN,
            craftFoundationBTN,
            craftWallBTN;


        #endregion

        //--------------------------------------------------------------Requirement Texts
        #region ("Requirement Texts")       
        Text AxeReq1,
            AxeReq2,
            PlankReq1,
            FoundationReq1,
            WallReq1;


        #endregion

        //--------------------------------------------------------------BluePrints
        #region ("Bluprint")       
        public Blueprint AxeBLP = new Blueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        public Blueprint PlankBLP = new Blueprint("Plank", 2, 1, "Log", 1, "", 0);
        public Blueprint FoundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 4, "", 0);
        public Blueprint WallBLP = new Blueprint("Wall", 1, 1, "Plank", 2, "", 0);


        #endregion



        public bool isOpen;


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

        //--------------------------------------------------------------Start
        void Start()
        {
            isOpen = false;

            // ũ������ UI "Tools"��ư reference
            toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
            toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

            // ũ������ UI "Survival"��ư reference
            survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
            survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

            // ũ������ UI "Refine"��ư reference
            refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
            refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

            // ũ������ UI "Construction"��ư reference
            constructionBTN = craftingScreenUI.transform.Find("ConstructButton").GetComponent<Button>();
            constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });



            // Axe �����
            AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
            AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

            craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
            craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

            // Plank �����           
            PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

            craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
            craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

            // Foundation �����
            FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

            craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
            craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });

            // Wall �����
            WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();

            craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
            craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });

        }

        // UI ���� �κ� (Tools, Survival, Refine) ��) void OpenToolsCategory
        #region
        void OpenToolsCategory()
        {
            craftingScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);

            toolScreenUI.SetActive(true);
        }

        void OpenSurvivalCategory()
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);

            survivalScreenUI.SetActive(true);
        }

        void OpenRefineCategory()
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);

            refineScreenUI.SetActive(true);
        }

        void OpenConstructionCategory()
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);

            constructionScreenUI.SetActive(true);
        }

        #endregion

        void CraftAnyItem(Blueprint blueprintToCraft)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

            StartCoroutine(craftedDelayForsound(blueprintToCraft));
        

            if (blueprintToCraft.numOfRequirements == 1)
            {
                // remove resources from intentory (b/c its used)
                InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            }
            else if (blueprintToCraft.numOfRequirements == 2) // if the item's requirementAmount is only 1, then it will make an error.
            {
                InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
                InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
            }

            // refresh list
            StartCoroutine(calculate());
        }

        public IEnumerator calculate()
        {
            yield return 0;

            InventorySystem.Instance.ReCalculateList();

            RefreshNeededItems();
        }

        // ũ����Ʈ �Ҹ��� �κ��丮�� ���� �Ҹ��� ���߱� ���� �����̸� ��. ( ���� ���� ����� �׷��� ���� �ƴ����� �׳� ������ )
        private IEnumerator craftedDelayForsound(Blueprint blueprintToCraft)
        {

            yield return new WaitForSeconds(1f);

            // produce the amount of items according to the blueprint
            for (var i = 0; i < blueprintToCraft.numOfItemsToProduce; i++)  // for������ �ʿ��� ����������ŭ �ݺ��ؼ� ����. ��) log�� craft�� 2���� �߰��ǹǷ� �ؿ� ���� 2�� ����
            {
                InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
            {
                Debug.Log("C is pressed");

                craftingScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // ����, �� �� �̷��� �Ⱥ��̰� ��
                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.C) && isOpen)
            {
                craftingScreenUI.SetActive(false);
                toolScreenUI.SetActive(false);
                survivalScreenUI.SetActive(false);
                refineScreenUI.SetActive(false);
                constructionScreenUI.SetActive(false);
                if (!InventorySystem.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }
                isOpen = false;
            }
        }

        public void RefreshNeededItems()
        {
            int stone_count = 0;
            int stick_count = 0;
            int log_count = 0;
            int plank_count = 0;

            inventoryItemList = InventorySystem.Instance.itemList;

            foreach (string itemName in inventoryItemList)
            {
                switch (itemName)
                {
                    case "Stone":
                        stone_count += 1;
                        break;

                    case "Stick":
                        stick_count += 1;
                        break;
                    case "Log":
                        log_count += 1;
                        break;
                    case "Plank":
                        plank_count += 1;
                        break;


                }
            }

            // ---- Axe ---- //
            AxeReq1.text = "3 Stone [" + stone_count + "]";
            AxeReq2.text = "3 Stick [" + stick_count + "]";

            // crafting button appear part
            if(stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))     // ep16.CheckSlotsAvailable �� �߰������ν� ����ĭ�� �����ÿ��� craft�ǵ���
            {
                craftAxeBTN.gameObject.SetActive(true);
            }
            else
            {
                craftAxeBTN.gameObject.SetActive(false);
            }

            // ---- Plank x2 ---- //
            PlankReq1.text = "1 Log [" + log_count + "]";           

            // crafting button appear part
            if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
            {
                craftPlankBTN.gameObject.SetActive(true);
            }
            else
            {
                craftPlankBTN.gameObject.SetActive(false);
            }

            // ---- Foundation ---- //
            FoundationReq1.text = "4 Log [" + plank_count + "]";

            // crafting button appear part
            if (plank_count >= 4 && InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                craftFoundationBTN.gameObject.SetActive(true);
            }
            else
            {
                craftFoundationBTN.gameObject.SetActive(false);
            }

            // ---- Wall ---- //
            WallReq1.text = "2 Log [" + plank_count + "]";

            // crafting button appear part
            if (plank_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                craftWallBTN.gameObject.SetActive(true);
            }
            else
            {
                craftWallBTN.gameObject.SetActive(false);
            }
        }
    }
}
