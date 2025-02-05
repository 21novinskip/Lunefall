using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign a UI Button prefab in the Inspector
    public Transform contentPanel;  // Assign the Content object of the Scroll View

    void Start()
    {
        GenerateList(7); // Creates 50 buttons
    }

    void GenerateList(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            //newButton.GetComponentInChildren<Text>().text = "Item " + (i + 1);
        }
    }
}
