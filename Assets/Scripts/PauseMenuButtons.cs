using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject settings_window;
    public string activePlayer;

    public Slider strSlider;
    public TMP_Text strText;
    public Slider agiSlider;
    public TMP_Text agiText;
    public Slider lckSlider;
    public TMP_Text lckText;
    public Slider defSlider;
    public TMP_Text defText;
    public Image menuPortrait;
    public Sprite Karlot;
    public Sprite Catalina;
    public Sprite Hildegard;

    void Start()
    {
        ShowKarlotStats();
    }

    public void ShowKarlotStats()
    {
        activePlayer = "p0";

        menuPortrait.sprite = Karlot;

        strSlider.value = GameManager.Instance.p0STR;
        strText.text = GameManager.Instance.p0STR.ToString();
        agiSlider.value = GameManager.Instance.p0AGI;
        agiText.text = GameManager.Instance.p0AGI.ToString();
        lckSlider.value = GameManager.Instance.p0LCK;
        lckText.text = GameManager.Instance.p0LCK.ToString();
        defSlider.value = GameManager.Instance.p0DEF;
        defText.text = GameManager.Instance.p0DEF.ToString();
    }
    public void ShowCatalinaStats()
    {
        activePlayer = "p1";

        menuPortrait.sprite = Catalina;

        strSlider.value = GameManager.Instance.p1STR;
        strText.text = GameManager.Instance.p1STR.ToString();
        agiSlider.value = GameManager.Instance.p1AGI;
        agiText.text = GameManager.Instance.p1AGI.ToString();
        lckSlider.value = GameManager.Instance.p1LCK;
        lckText.text = GameManager.Instance.p1LCK.ToString();
        defSlider.value = GameManager.Instance.p1DEF;
        defText.text = GameManager.Instance.p1DEF.ToString();
    }
    public void ShowHildegardStats()
    {
        activePlayer = "p2";

        menuPortrait.sprite = Hildegard;

        strSlider.value = GameManager.Instance.p2STR;
        strText.text = GameManager.Instance.p2STR.ToString();
        agiSlider.value = GameManager.Instance.p2AGI;
        agiText.text = GameManager.Instance.p2AGI.ToString();
        lckSlider.value = GameManager.Instance.p2LCK;
        lckText.text = GameManager.Instance.p2LCK.ToString();
        defSlider.value = GameManager.Instance.p2DEF;
        defText.text = GameManager.Instance.p2DEF.ToString();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
