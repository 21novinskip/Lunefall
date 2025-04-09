using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer vp;
    void Start()
    {
        vp = gameObject.GetComponent<VideoPlayer>();
        vp.Play();
        StartCoroutine (CheckVideoComplete());
    }
    IEnumerator CheckVideoComplete()
    {
        // Wait until the video starts playing
        while (!vp.isPlaying)
        {
            yield return null;
        }
        //now it is playing
        while (vp.isPlaying)
        {
            yield return null;
        }
        //bnow it is done
        SceneManager.LoadScene("StartVillage");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("StartVillage");
        }
    }
}
