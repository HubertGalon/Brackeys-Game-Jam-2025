using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public void startGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void gameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void youWin()
    {
        SceneManager.LoadScene("YouWin");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
