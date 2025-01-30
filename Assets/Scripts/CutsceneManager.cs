using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CutsceneImage
{
    public Sprite image; // Reference to an image
    [TextArea(1, 5)]
    public string caption; // Optional caption for the image
}

[System.Serializable]
public class Cutscene
{
    public List<CutsceneImage> cutsceneImage = new List<CutsceneImage>();
}

public class CutsceneManager : MonoBehaviour
{
    private Queue<CutsceneImage> scenes;
    public TextMeshProUGUI textSpace;
    public Cutscene cutscene;
    public string nextScene;
    // Start is called before the first frame update
    void Awake()
    {
        scenes = new Queue<CutsceneImage>();
        StartCutscene();
    }

    // Update is called once per frame
    void StartCutscene()
    {
        scenes.Clear();
        
        foreach (CutsceneImage cutsceneImage in cutscene.cutsceneImage)
        {
            scenes.Enqueue(cutsceneImage);
        }
        NewImage();
    }

    public void NewImage()
    {
        if (scenes.Count == 0)
        {
            SceneManager.LoadScene(nextScene);
            return;
        }

        CutsceneImage currentImage = scenes.Dequeue();
        GetComponent<Image>().sprite = currentImage.image;
        textSpace.text = currentImage.caption;

        StopAllCoroutines();

        StartCoroutine(CMNext());
    }

    IEnumerator CMNext()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Clicked enter.");
            NewImage();
            yield break;
        }
        yield return null;
    }
}
