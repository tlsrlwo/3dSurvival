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
            // M을 눌러서 메뉴UI 를 활성화, UI캔버스를 비활성화
            if(Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
            {
                uiCanvas.SetActive(false);
                menuCanvas.SetActive(true);

                isMenuOpen = true;

                // 메뉴를 실행시키면 커서잠금해제 & selection기능 비활성화
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            }
            else if(Input.GetKeyDown(KeyCode.M) && isMenuOpen)
            {
                // 메뉴에서 나갔다가 다시 들어갈 때 항상 menu화면이 나타나게 하기 위해 uiCanvas랑 menuCanvas 자체를 끄는 코드 전에 menu.setactive 를 설정해둠 
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

