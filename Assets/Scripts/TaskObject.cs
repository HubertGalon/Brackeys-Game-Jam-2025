using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    private bool taskActive = false;
    private TaskManager taskManager;
    public bool isPlayerInRange = false; 

    public void SetTaskActive(bool active, TaskManager manager)
    {
        taskActive = active;
        taskManager = manager;
    }

    void Update()
    {
        if (taskActive && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            taskManager.CompleteTaskStep();
        }
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
