using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer vp;
    public Image img;
    public string nextScene;
    void Start()
    {
        Debug.Log("Starting video");
        vp = gameObject.GetComponent<VideoPlayer>();
        vp.Play();
        StartCoroutine(CheckVideoComplete());
    }
    IEnumerator CheckVideoComplete()
    {
        vp.Play();
        Debug.Log("Trying to play video...");

        while (!vp.isPlaying)
        {
            Debug.Log("Waiting for video to start...");
            yield return null;
        }

        Debug.Log("Video started!");

        while (vp.isPlaying)
        {
            if (img.color.a > 0)
            {
                Color newColor = img.color;
                newColor.a -= 0.001f;
                img.color = newColor;
            }
            yield return null;
        }

        Debug.Log("Video finished, loading next scene.");
        SceneManager.LoadScene(nextScene);
    }
}
