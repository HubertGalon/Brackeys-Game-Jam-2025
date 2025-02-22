using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private SceneChanger sceneChanger;
    private TaskManager taskManager;
    public void GameReset()
    {
        taskManager.currentQuest.enabled = false;
        taskManager.activeNPC.HideTaskBubble();
        taskManager.activeNPC = null;
        taskManager.hasTask = false;
        taskManager.taskAccepted = false;
        taskManager.taskCompleted = false;
        taskManager.currentQuest.enabled = false;
        taskManager.animator.SetBool("pizzaTask", false);
        taskManager.animator.SetBool("beerTask", false);
        
        //sceneChanger.mainMenu();
    }
}
