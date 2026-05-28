using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelectMenu");
        Debug.Log("in the function");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenLevel(string levelName)
    {
        Debug.Log("Opening level " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void PlayNextLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        string nextLevelName = "Level" + (Convert.ToInt32(activeScene.name.Split("Level")[^1]) + 1);
        Debug.Log("next level to be loaded: " + nextLevelName);

        SceneManager.LoadScene(nextLevelName);

        if (!SceneManager.GetSceneByName(nextLevelName).IsValid())
        {
            SceneManager.LoadScene("LevelSelectMenu");
        }
    }
}