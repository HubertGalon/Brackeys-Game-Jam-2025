using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public bool hasTask = false;
    public NPC[] npcs;
    public Sprite[] taskIcons;
    public TaskObject table;
    public TaskObject waterMachine;
    public TaskObject stain;
    public TaskObject DJ;
    private int taskNum;
    public NPC activeNPC;
    public bool taskAccepted = false;
    public bool taskCompleted = false;
    public bool taskFinished = false;
    public Transform[] stainSpawnPoints;
    public GameObject stainPrefab;
    private GameObject currentStain;

    private Coroutine cancelTaskCoroutine;

    private Coroutine taskCoroutine;

    private Coroutine timeoutCoroutine;
    public Image currentQuest;
    public ScoreManager scoreManager;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip[] backgroundMusic;
    void Start()
    {
        currentQuest.enabled = false;
        animator.SetBool("pizzaTask", false);
    }
    void Update()
    {
        if (!hasTask && taskCoroutine == null)
        {
            taskCoroutine = StartCoroutine(AssignTaskWithDelay());
        }

        if (!taskAccepted && activeNPC != null && activeNPC.isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AcceptTask();
        }

        if (taskAccepted && !taskCompleted && Input.GetKeyDown(KeyCode.E))
        {
            if (taskNum == 0 && table.isPlayerInRange)
            {
                CompleteTaskStep();
            }
            else if (taskNum == 1 && waterMachine.isPlayerInRange)
            {
                CompleteTaskStep();
            }
            else if (taskNum == 2 && stain.isPlayerInRange)
            {
                FinishTask();
            }
            else if (taskNum == 3 && DJ.isPlayerInRange)
            {
                CompleteTaskStep();
            }
        }

        if (taskCompleted && activeNPC.isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            //activeNPC = null;
            FinishTask();
        }
    }


    IEnumerator AssignTaskWithDelay()
    {
        float delay = Random.Range(1f, 3f);
        yield return new WaitForSeconds(delay);
        AssignRandomTask();
        taskCoroutine = null;
    }
    void AssignRandomTask()
    {
        taskFinished = false;
        hasTask = true;
        taskAccepted = false;
        taskCompleted = false;

        taskNum = Random.Range(0, 4);
        activeNPC = npcs[Random.Range(0, npcs.Length)];

        if (activeNPC == null)
        {
            Debug.LogError("Błąd: Nie wylosowano NPC!");
            return;
        }

        Debug.Log("Zadanie przypisane do: " + activeNPC.name);
        activeNPC.ShowTaskBubble(taskIcons[taskNum]);
        cancelTaskCoroutine = StartCoroutine(CancelTaskAfterDelay(3f));
    }

    void AcceptTask()
    {
        Debug.Log("Gracz zaakceptował zadanie!");
        taskAccepted = true;
        activeNPC.HideTaskBubble();
        currentQuest.enabled = true;
        currentQuest.sprite = taskIcons[taskNum];

        if (cancelTaskCoroutine != null)
        {
            StopCoroutine(cancelTaskCoroutine);
        }

        timeoutCoroutine = StartCoroutine(CancelTaskAfterTimeout(3f));

        switch (taskNum)
        {
            case 0: // Jedzenie
                table.SetTaskActive(true, this);
                break;
            case 1: // Woda
                waterMachine.SetTaskActive(true, this);
                break;
            case 2: // Plamy
                if (stainSpawnPoints.Length > 0 && stainPrefab != null)
                {
                    int randomIndex = Random.Range(0, stainSpawnPoints.Length);
                    Transform spawnPoint = stainSpawnPoints[randomIndex];
                    currentStain = Instantiate(stainPrefab, spawnPoint.position, Quaternion.identity);
                    currentStain.transform.position = new Vector3(currentStain.transform.position.x, currentStain.transform.position.y, 0f);

                    stain = currentStain.GetComponent<TaskObject>();

                    if (stain != null)
                    {
                        stain.SetTaskActive(true, this);
                    }
                }
                break;
            case 3: // DJ
                DJ.SetTaskActive(true, this);
                break;
        }
    }

    public void CompleteTaskStep()
    {
        Debug.Log("Gracz wykonał interakcję przy stole!");
        taskCompleted = true;

        if (timeoutCoroutine != null)
        {
            StopCoroutine(timeoutCoroutine);
        }
        timeoutCoroutine = StartCoroutine(CancelTaskAfterTimeout(3f));
        switch (taskNum)
        {
            case 0:
                table.SetTaskActive(false, this);
                animator.SetBool("pizzaTask", true);
                break;
            case 1:
                waterMachine.SetTaskActive(false, this);
                animator.SetBool("beerTask", true);
                break;
            case 3:
                DJ.SetTaskActive(false, this);
                AudioClip newClip;
                do{
                    newClip = backgroundMusic[Random.Range(0, backgroundMusic.Length)];
                } while (audioSource.clip == newClip);
                audioSource.clip = newClip;
                audioSource.Play();
                break;
        }
        
        currentQuest.sprite = taskIcons[5];
        activeNPC.ShowTaskBubble(taskIcons[5]);
    }

    void FinishTask()
    {
        Debug.Log("Gracz wrócił do NPC i ukończył zadanie!");

        if (timeoutCoroutine != null)
        {
            StopCoroutine(timeoutCoroutine);
        }
        taskFinished = true;
        if (taskNum == 2)
        {
            stain.SetTaskActive(false, this);
            if (currentStain != null)
            {
                Destroy(currentStain);
            }
        }
        currentQuest.enabled = false;
        activeNPC.HideTaskBubble();
        activeNPC = null;
        hasTask = false;
        taskAccepted = false;
        taskCompleted = false;
        scoreManager.AddScore();
        currentQuest.enabled = false;
        animator.SetBool("pizzaTask", false);
        animator.SetBool("beerTask", false);
    }


    IEnumerator CancelTaskAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!taskAccepted)
        {
            Debug.Log("Zadanie zostało anulowane, ponieważ nie zostało zaakceptowane na czas!");
            activeNPC.HideTaskBubble();
            hasTask = false;
            scoreManager.punishScore();
            currentQuest.enabled = false;
        }
    }
    
    IEnumerator CancelTaskAfterTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!taskCompleted || !taskFinished)
        {
            Debug.Log("Zadanie zostało anulowane, ponieważ nie zostało wykonane na czas!");
            if (activeNPC != null)
            {
                activeNPC.HideTaskBubble();
            }

            if (taskNum == 2 && currentStain != null) 
            {
                Destroy(currentStain); 
                currentStain = null;
            }
            scoreManager.punishScore();
            hasTask = false;
            taskAccepted = false;
            taskCompleted = false;
            activeNPC = null;
            currentQuest.enabled = false;
            animator.SetBool("pizzaTask", false);
            animator.SetBool("beerTask", false);
        }
    }
}
