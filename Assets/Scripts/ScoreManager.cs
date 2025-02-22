using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public RectTransform scoreBar; 
    public float maxScore = 100f; 
    public float decreaseRate = 1f; 
    public float increaseAmount = 5f;
    public float timeMultiplier = 0.5f; 
    public float maxDecreaseRate = 5f;
    public float increaseRateDelay = 10f; 

    private float currentScore;
    private float originalWidth;
    private Vector2 originalPosition;
    private float elapsedTime = 0f;

    public Sprite[] scoreSprites;
    public Image ScoreBar;
    public SceneChanger sceneChanger;
    void Start()
    {
        currentScore = maxScore;
        originalWidth = scoreBar.sizeDelta.x;
        originalPosition = scoreBar.anchoredPosition;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > increaseRateDelay)
        {
            float difficultyFactor = Mathf.Log(elapsedTime - increaseRateDelay + 1, 10) * timeMultiplier;
            decreaseRate += difficultyFactor * Time.deltaTime *3f;
            decreaseRate = Mathf.Min(decreaseRate, maxDecreaseRate); 
        }

        currentScore -= decreaseRate * Time.deltaTime;
        currentScore = Mathf.Clamp(currentScore, 0, maxScore);

        UpdateScoreBar();
        if (currentScore > maxScore/2){
            ScoreBar.sprite = scoreSprites[0];
        }
        else if (currentScore < maxScore/2 && currentScore >= maxScore/3){
            ScoreBar.sprite = scoreSprites[1];
        }
        else{
            ScoreBar.sprite = scoreSprites[2];
        }
        if (currentScore <= 0)
        {
            GameOver();
        }
    }

    void UpdateScoreBar()
    {
        float newWidth = Mathf.Min((currentScore / maxScore) * originalWidth, originalWidth);
        float positionOffset = originalWidth - newWidth;
        scoreBar.sizeDelta = new Vector2(newWidth, scoreBar.sizeDelta.y);
    }

    public void AddScore()
    {
        currentScore += increaseAmount*(Mathf.Abs(decreaseRate/2));
        currentScore = Mathf.Clamp(currentScore, 0, maxScore);
    }

    public void punishScore()
    {
        currentScore -= increaseAmount*(Mathf.Abs(decreaseRate));
        currentScore = Mathf.Clamp(currentScore, 0, maxScore);
    }
    void GameOver()
    {
        sceneChanger.gameOver();
    }
}
