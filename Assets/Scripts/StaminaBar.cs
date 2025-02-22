using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    public RectTransform staminaBar; 
    public float maxStamina = 2f;
    private float originalWidth;
    private Vector2 originalPosition;
    public PlayerMovement player;
    
    void Start()
    {
        originalWidth = staminaBar.sizeDelta.x; 
        originalPosition = staminaBar.anchoredPosition;
    }

    void Update()
    {
        if (player != null)
        {
            UpdateStaminaBar();
        }
    }

    void UpdateStaminaBar()
    {
        float newWidth = (player.currentSprintTime / maxStamina) * originalWidth;
        float positionOffset = originalWidth - newWidth;
        staminaBar.anchoredPosition = new Vector2(originalPosition.x - positionOffset / 2, originalPosition.y);
        staminaBar.sizeDelta = new Vector2(newWidth, staminaBar.sizeDelta.y); 
    }
}
