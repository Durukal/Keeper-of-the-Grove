using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Tutorials()
    {
        SceneManager.LoadScene("Tutorials");
    }

    public void QuitGame()
    {
        Application.Quit();
    } 

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
