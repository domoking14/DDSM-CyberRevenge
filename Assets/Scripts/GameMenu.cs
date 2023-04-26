using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ResumeGame()
    {
        if(UIManager.UIM.prevState == 0)
        {
            UIManager.UIM.gameState = 0;
        }
        else if (UIManager.UIM.prevState == 1)
        {
            UIManager.UIM.gameState = 1;
        }
    }

    //loads selected level
    public void LoadMenu()
    {
        //Load level with scene name, load menu
        SceneManager.LoadScene("Main Menu");
    }
    public void BackMenu()
    {
        UIManager.UIM.gameState = 0;
    }
    public void LevelSelect()
    {
        //Load level select
        UIManager.UIM.gameState = 1;
    }

    public void Level1()
    {
        SceneManager.LoadScene("DK Level");
    }
    public void Level2()
    {
        SceneManager.LoadScene("DA Level");
    }
    public void Level3()
    {
        SceneManager.LoadScene("SD Level");
    }

    //quit game on button press
    public void ExitGame()
    {
        Application.Quit();
    }
}
