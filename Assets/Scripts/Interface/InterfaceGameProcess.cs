using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


using TMPro;

namespace Platformer2D
{
    public class GameOver : MonoBehaviour
    {
        public GameObject panelGameWint;

        public GameObject panel;
        public GameObject panelSettings;
        public Button ButtonExit;
        public Button GameOverButton;
        public Button GameWinButton;
        //public UnityEngine.UI.Button returnToGame;

        //public UnityEngine.UI.Button Esc;
        public TextMeshProUGUI textTotalScore;

        private void Start()
        {
            // Подписываемся на событие нажатия кнопки
            GameOverButton.onClick.AddListener(OnOkButtonClicked);
            GameWinButton.onClick.AddListener(OnOkButtonClicked);
            ButtonExit.onClick.AddListener(OnOkButtonClicked);

            Bus.Instance.PlayerFell +=OnPlayerFell;
            Bus.Instance.GameWin += GameCompleted;
            //UnityEngine.Cursor.visible = false;
        }
        private void OnDestroy()
        {
            GameOverButton.onClick.RemoveListener(OnOkButtonClicked);
            GameWinButton.onClick.RemoveListener(OnOkButtonClicked);
            Bus.Instance.PlayerFell -= OnPlayerFell;
            Bus.Instance.GameWin -= GameCompleted;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Переключает активность панели при нажатии клавиши Esc
                panelSettings.SetActive(!panelSettings.activeSelf);
                //UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
            }
        }

        private void DestroyAllObjects()
        {
            // Получаем активную сцену
            Scene activeScene = SceneManager.GetActiveScene();

            // Получаем все корневые объекты в сцене
            GameObject[] rootObjects = activeScene.GetRootGameObjects();

            // Проходим по каждому корневому объекту и его дочерним объектам
            foreach (GameObject rootObject in rootObjects)
            {
                Destroy(rootObject);
            }
        }

        private void OnOkButtonClicked()
        {
            //SceneManager.UnloadSceneAsync("TestScene");
            //DestroyAllObjects();
            //DBus.Instance.Destroy();
            SceneManager.LoadScene("StartScene");
        }

        private void OnPlayerFell(bool isFell)
        {
            if (isFell)
            {
                Debug.Log("gameover");
                panel.SetActive(true);
                //UnityEngine.Cursor.visible = false;

            }
        }

        private void  GameCompleted(int count)
        {
            panelGameWint.gameObject.SetActive(true);
            textTotalScore.text ="Game win with score: " + count.ToString() ;
            UnityEngine.Cursor.visible = true;
        }

        
    }
}

