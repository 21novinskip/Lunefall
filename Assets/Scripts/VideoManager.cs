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
    void Start()
    {
        vp = gameObject.GetComponent<VideoPlayer>();
        //vp.Play();
        StartCoroutine(CheckVideoComplete());
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
            //Kills our overlay screen slowly (fade)
            if (img.color.a > 0)
            {
                Color newColor = img.color;
                newColor.a -= 0.001f;
                img.color = newColor;
            }
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
