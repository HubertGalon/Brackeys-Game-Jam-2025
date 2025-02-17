using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public bool hasTask = false;
    public NPC[] npcs;
    public Sprite[] taskIcons;
    public TaskObject table; 

    public TaskObject waterMachine;
    
    public TaskObject stain;
    private int taskNum;
    private NPC activeNPC; 
    private int currentTask;
    private Coroutine cancelTaskCoroutine; 
    private bool taskAccepted = false; 
    private bool taskCompleted = false; 


    public Transform[] stainSpawnPoints;
    public GameObject stainPrefab;
    private GameObject currentStain; 

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

        if (taskAccepted && !taskCompleted && Input.GetKeyDown(KeyCode.E))
        {
            if (taskNum == 0){
                if (table.isPlayerInRange){
                    CompleteTaskStep();
                }
            }
            if (taskNum == 1){
                if (waterMachine.isPlayerInRange){
                    CompleteTaskStep();
                }
            }
            if (taskNum == 2){
                if (stain.isPlayerInRange){
                    FinishTask();
                }
            }
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

        
        taskNum = Random.Range(0, 3);
        activeNPC = npcs[Random.Range(0, npcs.Length)]; 

        if (activeNPC == null)
        {
            Debug.LogError("Błąd: Nie wylosowano NPC!");
            return;
        }

        Debug.Log("Zadanie przypisane do: " + activeNPC.name);
        activeNPC.ShowTaskBubble(taskIcons[taskNum]); 
    }

    void AcceptTask()
    {
        Debug.Log("Gracz zaakceptował zadanie!");
        taskAccepted = true; 
        activeNPC.HideTaskBubble(); 
        switch(taskNum){
            case 0:{ // jedzenie
                table.SetTaskActive(true, this);
            }
            break;
            case 1:{ // woda
                waterMachine.SetTaskActive(true, this);
            }
            break;
            case 2:{ // plamy
                if (stainSpawnPoints.Length > 0 && stainPrefab != null)
                {
                    int randomIndex = Random.Range(0, stainSpawnPoints.Length);
                    Transform spawnPoint = stainSpawnPoints[randomIndex];
                    currentStain = Instantiate(stainPrefab, spawnPoint.position, Quaternion.identity);
                    Vector3 stainPosition = currentStain.transform.position;
                    stainPosition.z = 0f;
                    currentStain.transform.position = stainPosition;
                    stain = currentStain.GetComponent<TaskObject>();
                    if (stain != null)
                        {
                            stain.SetTaskActive(true, this);
                        }
                    else
                        {
                            Debug.LogError("Błąd: Stworzona plama nie ma komponentu TaskObject!");
                        }
                }
            }
            break;
        }
    }

    public void CompleteTaskStep()
    {
        Debug.Log("Gracz wykonał interakcję przy stole!");
        taskCompleted = true; 
        
        switch(taskNum){
            case 0:
                table.SetTaskActive(false, this);
            break;
            case 1:
                waterMachine.SetTaskActive(false, this);
            break;
        }
        activeNPC.ShowTaskBubble(taskIcons[5]); 
    }

    void FinishTask()
    {
        Debug.Log("Gracz wrócił do NPC i ukończył zadanie!");
        if (taskNum == 2){
            stain.SetTaskActive(false, this);
            if (currentStain != null)
                {
                    Destroy(currentStain);
                }
        }
        activeNPC.HideTaskBubble();
        hasTask = false;
        taskAccepted = false;
        taskCompleted = false;
    }
}
