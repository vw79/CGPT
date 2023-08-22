using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public void GoToStartScene()
    {
        // Load the start scene
        SceneManager.LoadScene("StartScene");
    }

    public void GoToFirstScene()
    {
        // Load the first scene
        SceneManager.LoadScene("Prison");
    }
    public void StopGame()
    {
        // If in a built game, quit the application
        Application.Quit();
    }
}