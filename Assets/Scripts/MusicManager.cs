using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource snd_music1; // First audio source
    public AudioSource snd_music2; // Second audio source

    public int Situation = 1; // Controls which audio source is played (1 or 2)

    private void Awake()
    {
        // Ensure this object persists between scene loads
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Ensure the GameObject has two AudioSource components
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            snd_music1 = gameObject.AddComponent<AudioSource>();
            snd_music2 = null;
            snd_music1.loop = true;
            snd_music1.Play();
        }
        else
        {
            snd_music1 = audioSources[0];
            snd_music2 = audioSources[1];
            snd_music1.loop = true;
            snd_music2.loop = true;
        }

        // Set up audio properties


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loadedddd");
        //calls specific setup function based on which scene is loaded
        if (scene.name == "BattleScene")
        {
            PlayMusicBasedOnSituation();
        }
        else
        {
            snd_music1.Play();
        }
        //add as many if statements and functions as there are overworld scenes
    }

    private void OnEnable()
    {
        // Register the scene change event to stop the music
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // Unregister the scene change event when disabled
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene currentScene)
    {
        // Stop the audio and destroy the GameObject when the scene is unloaded
        //snd_music1.Stop();
        if (snd_music2 != null)
        {
            snd_music2.Stop();
        }
        Destroy(gameObject);
    }

    public void PlayMusicBasedOnSituation()
    {
        // Stop both tracks first
        //snd_music1.Stop();
        if (snd_music2 != null)
        {
            snd_music2.Stop();
        }

        // Play the appropriate audio based on the "Situation" variable
        if (Situation == 1)
        {
            snd_music1.Play();
        }
        else if (Situation == 2)
        {
            snd_music2.Play();
        }
        else
        {
            Debug.LogWarning("Invalid Situation value! Must be 1 or 2.");
        }
    }

    // Optionally, call this method to change the situation dynamically
    public void SetSituation(int newSituation)
    {
        Situation = newSituation;
        PlayMusicBasedOnSituation();
    }
}
