using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject taskBubble;
    public Image taskIcon;
    public bool isPlayerInRange = false;

    void Start()
    {
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
