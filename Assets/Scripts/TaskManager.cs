using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public bool hasTask = false;
    public NPC[] npcs;
    public Sprite[] taskIcons;
    public TaskObject table; 
    
    private NPC activeNPC; 
    private int currentTask;
    private Coroutine cancelTaskCoroutine; 
    private bool taskAccepted = false; 
    private bool taskCompleted = false; 

    void Update()
    {
        if (!hasTask)
        {
            AssignRandomTask(); 
        }


        if (!taskAccepted && activeNPC != null && activeNPC.isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AcceptTask();
        }

        if (taskAccepted && !taskCompleted && table.isPlayerInRange && Input.GeKetyDown(KeyCode.E))
        {
            CompleteTaskStep();
        }


        if (taskCompleted && activeNPC.isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            FinishTask();
        }
    }

    void AssignRandomTask()
    {
        hasTask = true;
        taskAccepted = false;
        taskCompleted = false;

        
        int taskNum = 1;
        activeNPC = npcs[Random.Range(0, npcs.Length)]; 

        if (activeNPC == null)
        {
            Debug.LogError("Błąd: Nie wylosowano NPC!");
            return;
        }

        Debug.Log("Zadanie przypisane do: " + activeNPC.name);
        activeNPC.ShowTaskBubble(taskIcons[0]); 
    }

    void AcceptTask()
    {
        Debug.Log("Gracz zaakceptował zadanie!");
        taskAccepted = true; 
        activeNPC.HideTaskBubble(); 
        table.SetTaskActive(true, this);
    }

    public void CompleteTaskStep()
    {
        Debug.Log("Gracz wykonał interakcję przy stole!");
        taskCompleted = true; 
        table.SetTaskActive(false, this); 
        activeNPC.ShowTaskBubble(taskIcons[5]); 
    }

    void FinishTask()
    {
        Debug.Log("Gracz wrócił do NPC i ukończył zadanie!");
        activeNPC.HideTaskBubble();
        hasTask = false;
        taskAccepted = false;
        taskCompleted = false;
    }
}
