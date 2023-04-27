using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager UIM;
    public TextMeshProUGUI currentLives;

    private GameObject[] pauseOBJs;
    private GameObject[] gameOverOBJs;
    private GameObject[] LVcompleteOBJs;
    public GameObject[] GameStates; //<-- I could play with this so that i decrease the number of scenes i am using (fully)
    public GameObject curState;
    public int gameState = 0;
    public int prevState;

    private GameObject[] MainMenuOBJs;
    private GameObject[] LevelSelectOBJs;

    private bool OnMenu = false;
    private bool InGame = false;
    private Scene curScene;
    private void Awake()
    {
        if(UIM == null)
        {
            UIM = this;
        }

        curScene = SceneManager.GetActiveScene();
        if (curScene.name == "Main Menu")
        {
            OnMenu = true;
            print("On Menu");
        }

        else if (curScene.name != "Main Menu")
        {
            InGame = true;
            print("In Game");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (InGame)
        {
            foreach(TextMeshProUGUI txt in GetComponentsInChildren<TextMeshProUGUI>())
            {
                switch (txt.gameObject.name)
                {
                    case "Lives":
                        currentLives = txt;
                        break;
                }
            }

            gameState = 0;
            pauseOBJs = GameObject.FindGameObjectsWithTag("ShowOnPause");
            gameOverOBJs = GameObject.FindGameObjectsWithTag("ShowOnFail");
            LVcompleteOBJs = GameObject.FindGameObjectsWithTag("ShowOnComplete");
            curState = GameStates[gameState];
        }

        if (OnMenu)
        {
            gameState = 0;
            MainMenuOBJs = GameObject.FindGameObjectsWithTag("ShowOnMenu");
            LevelSelectOBJs = GameObject.FindGameObjectsWithTag("ShowOnLVselect");
            curState = GameStates[gameState];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InGame)
        {
            switch (gameState)
            {
                case 0:
                    HidePause();
                    HideLVcomplete();
                    HideGameOver();
                    curState = GameStates[0];
                    prevState = 0;
                    Time.timeScale = 1;
                    break;

                case 1:
                    ShowPause();
                    HideLVcomplete();
                    HideGameOver();
                    curState = GameStates[1];
                    prevState = 1;
                    Time.timeScale = 0;
                    break;

                case 2:
                    HidePause();
                    ShowLVcomplete();
                    HideGameOver();
                    curState = GameStates[2];
                    prevState = 2;
                    Time.timeScale = 0;
                    break;

                case 3:
                    HidePause();
                    HideLVcomplete();
                    ShowGameOver();
                    curState = GameStates[3];
                    prevState = 3;
                    Time.timeScale = 0;
                    break;
            }
        }
        if (OnMenu)
        {
            switch (gameState)
            {
                case 0:
                    ShowMainMenu();
                    HideLVselect();
                    curState = GameStates[0];
                    prevState = 0;
                    break;

                case 1:
                    HideMainMenu();
                    ShowLVselect();
                    curState = GameStates[1];
                    prevState = 1;
                    break;
            }
        }
    }

    private void ShowMainMenu()
    {
        foreach(GameObject s in MainMenuOBJs)
        {
            s.SetActive(true);
        }
    }
    private void HideMainMenu()
    {
        foreach (GameObject s in MainMenuOBJs)
        {
            s.SetActive(false);
        }
    }
    private void ShowLVselect()
    {
        foreach (GameObject s in LevelSelectOBJs)
        {
            s.SetActive(true);
        }
    }
    private void HideLVselect()
    {
        foreach (GameObject s in LevelSelectOBJs)
        {
            s.SetActive(false);
        }
    }
    private void ShowGameOver()
    {
        foreach (GameObject s in gameOverOBJs)
        {
            s.SetActive(true);
        }
    }
    private void HideGameOver()
    {
        foreach (GameObject s in gameOverOBJs)
        {
            s.SetActive(false);
        }
    }
    private void ShowLVcomplete()
    {
        foreach (GameObject s in LVcompleteOBJs)
        {
            s.SetActive(true);
        }
    }
    private void HideLVcomplete()
    {
        foreach (GameObject s in LVcompleteOBJs)
        {
            s.SetActive(false);
        }
    }
    private void ShowPause()
    {
        foreach (GameObject s in pauseOBJs)
        {
            s.SetActive(true);
        }
    }
    private void HidePause()
    {
        foreach (GameObject s in pauseOBJs)
        {
            s.SetActive(false);
        }
    }

    public void UpdateLives(int livesUP)
    {
        currentLives.text = "Lives: " + livesUP;
    }
}
