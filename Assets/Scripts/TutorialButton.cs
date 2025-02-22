using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public Image tutorial;
    public void showTutorial()
    {
        tutorial.gameObject.SetActive(true);
    }
    public void closeTutorial()
    {
        tutorial.gameObject.SetActive(false);
    }
}
