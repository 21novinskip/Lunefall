using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{

    public string WhereTo;

    // This function is called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger has the "Player" tag
        if ((collision.tag == "fighterOW") || (collision.tag == "tankOW") || (collision.tag == "witchOW"))
        {
            var thisScene = SceneManager.GetActiveScene().name;
            GameManager.Instance.previousSceneOW = thisScene;

            if (thisScene == "StartVillage" && GameManager.Instance.tutorialFinished == false)
            {
                Debug.Log("No.");
            }
            // Load the scene named "Forest"
            else
            {
                SceneManager.LoadScene(WhereTo);
            }
        }
    }
}
