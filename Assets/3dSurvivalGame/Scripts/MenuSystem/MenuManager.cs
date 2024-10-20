 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance { get; set; }


        #region("References")
        public GameObject menuCanvas;
        public GameObject uiCanvas;

        public GameObject saveMenu;
        public GameObject settingsMenu;
        public GameObject menu;

        public bool isMenuOpen;

        #endregion

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

        private void Update()
        {
            // M�� ������ �޴�UI �� Ȱ��ȭ, UIĵ������ ��Ȱ��ȭ
            if(Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
            {
                uiCanvas.SetActive(false);
                menuCanvas.SetActive(true);

                isMenuOpen = true;

                // �޴��� �����Ű�� Ŀ��������� & selection��� ��Ȱ��ȭ
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            }
            else if(Input.GetKeyDown(KeyCode.M) && isMenuOpen)
            {
                // �޴����� �����ٰ� �ٽ� �� �� �׻� menuȭ���� ��Ÿ���� �ϱ� ���� uiCanvas�� menuCanvas ��ü�� ���� �ڵ� ���� menu.setactive �� �����ص� 
                saveMenu.SetActive(false);
                settingsMenu.SetActive(false);
                menu.SetActive(true);


                uiCanvas.SetActive(true);
                menuCanvas.SetActive(false);

                isMenuOpen = false;

                if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
        }


        public void TempSaveGame()
        {
            SaveManager.Instance.SaveGame();
        }



    }
}

