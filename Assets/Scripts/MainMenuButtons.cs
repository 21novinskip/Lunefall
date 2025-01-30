using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject settings_window;
    void Awake()
    {
        GameManager.Instance.resetThings();
        GameManager.Instance.tutorialFinished = false;
        GameManager.Instance.tutorialJUSTFinished = false;
        GameManager.Instance.previousScene = null;
        GameManager.Instance.previousSceneOW = null;
    }
    public void GoToGame()
    {
        SceneManager.LoadScene("cutScene");
    }
    public void OpenSettings()
    {
        settings_window.SetActive(true);
    }
    public void CloseSettings()
    {
        settings_window.SetActive(false);
    }
    public void QuitGame()
    {
        // If running in the Unity Editor, this won't quit but will print a message.
        #if UNITY_EDITOR
        Debug.Log("Game is exiting (This won't close the game in the Unity Editor).");
        #else
        Application.Quit(); // Quits the application
        #endif
    }
}

