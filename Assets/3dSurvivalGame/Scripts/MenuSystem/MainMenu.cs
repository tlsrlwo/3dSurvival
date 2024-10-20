using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SUR
{
    public class MainMenu : MonoBehaviour
    {
        public Button LoadGameBTN;


        private void Start()
        {
            LoadGameBTN.onClick.AddListener(() =>
            {
                SaveManager.Instance.LoadGameWhenGameStarts();
            });
        }

        public void NewGame()
        {
            SceneManager.LoadScene("GameScene");
        }
        public void ExitGame()
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
    }
}
