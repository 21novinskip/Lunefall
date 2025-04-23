using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource snd_music1; 

    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " loaded.");
        snd_music1.Stop();
//calls battle music (variable)
        if (scene.name == "BattleScene")
        {
            snd_music1.clip = GameManager.Instance.battleMusic;
            snd_music1.Play(); 
        }
        if (scene.name == "BattleTutorial")
        {
            snd_music1.clip = GameManager.Instance.tutorialMusic;
            snd_music1.Play(); 
        }
//calls title music (static)
        if (scene.name == "MainMenu")
        {
            snd_music1.clip = GameManager.Instance.titleMusic;
            snd_music1.Play(); 
        }
//calls village music (static)
        if (scene.name == "StartVillage")
        {
            snd_music1.clip = GameManager.Instance.startVillageMusic;
            snd_music1.Play(); 
        }
//calls forest music (static)
        if (scene.name == "Forest")
        {
            snd_music1.clip = GameManager.Instance.forestMusic;
            snd_music1.Play(); 
        }
        if (scene.name == "GraveyardMT" || scene.name == "BossRoom")
        {
            snd_music1.clip = GameManager.Instance.graveyardMusic;
            snd_music1.Play();
        }
    }
}
