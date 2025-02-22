using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    private float gameTime = 0f;
    public float timeMultiplier = 2f;  
    public SceneChanger sceneChanger;
    void Update()
    {
        gameTime += Time.deltaTime * timeMultiplier;

        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        if (seconds % 15 == 0)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (minutes != 0 && minutes % 6 == 0)
        {
           endGame();
        }
    }
    void endGame()
    {
        sceneChanger.youWin();
    }
}
