using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TransState{FADING_IN, FADING_OUT, WAITING, }

public class SceneTransition : MonoBehaviour
{
    private Image img;
    public TransState tState;
    private float ticker = 0f;  // Use float to smoothly increment over time
    private float tickMax = 1f;  // Duration of the fade-in (in seconds)
    public GameObject meMyVerySelf;

    // Start is called before the first frame update
    void Start()
    {
        meMyVerySelf.SetActive(false);
        img = GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);  // Start with transparent image
    }

    void FadeOut()
    {
        // Increment ticker over time based on frame delta time
        if (ticker < tickMax)
        {
            ticker += Time.deltaTime / tickMax;
            img.color = new Color(img.color.r, img.color.g, img.color.b, ticker);  // Gradually increase alpha
        }
        else
        {
            tState = TransState.WAITING;
        }
    }
    void FadeIn()
    {
        // Increment ticker over time based on frame delta time
        if (ticker > 0.0f)
        {
            ticker -= Time.deltaTime / tickMax;
            img.color = new Color(img.color.r, img.color.g, img.color.b, ticker);  // Gradually increase alpha
        }
        else
        {
            tState = TransState.WAITING;
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch(tState)
        {
            case TransState.WAITING:
                break;
            case TransState.FADING_IN:
                FadeIn();
                break;
            case TransState.FADING_OUT:
                FadeOut();
                break;
        }
    }
    public void StartFadeOut()
    {
        tState = TransState.FADING_OUT;
        ticker = 0f;  // Reset ticker for fade out
        img.color = new Color(img.color.r, img.color.g, img.color.b, ticker);  // Reset alpha to 0
    }

    // Method to start fading in (triggered externally)
    public void StartFadeIn()
    {
        tState = TransState.FADING_IN;
        //ticker = 0f;  // Reset ticker for fade in (to start from fully opaque)
        img.color = new Color(img.color.r, img.color.g, img.color.b, ticker);  // Set alpha to 1
    }
    public IEnumerator ExitingTransition(System.Action onComplete)
    {
        if (meMyVerySelf.activeInHierarchy != true)
        {
            meMyVerySelf.SetActive(true);
        }
        StartFadeOut();
        yield return new WaitForSeconds(1);
        onComplete?.Invoke();
    }
    public IEnumerator EnteringTransition()
    {
        StartFadeIn();
        yield return new WaitForSeconds(1);
        meMyVerySelf.SetActive(false);
    }
}
