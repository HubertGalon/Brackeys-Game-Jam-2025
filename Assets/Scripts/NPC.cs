using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject taskBubble;
    public Image taskIcon;
    public bool isPlayerInRange = false;

    public SpriteRenderer spriteRenderer; 
    public Sprite[] NPCSprites;
    void Start()
    {
        if (NPCSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, NPCSprites.Length); 
            spriteRenderer.sprite = NPCSprites[randomIndex];
        }
        taskBubble.SetActive(false); 
    }

    public void ShowTaskBubble(Sprite icon)
    {
        taskBubble.SetActive(true);
        taskIcon.sprite = icon; 
    }

    public void HideTaskBubble()
    {
        taskBubble.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
