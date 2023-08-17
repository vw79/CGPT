using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchButton : MonoBehaviour
{
    public void GoToStartScene()
    {
        // Load the start scene
        SceneManager.LoadScene("StartScene");
    }

    public void GoToFirstScene()
    {
        // Load the first scene
        SceneManager.LoadScene("Animation Test");
    }

}
