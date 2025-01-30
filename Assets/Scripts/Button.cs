using System;
using UnityEngine;
using UnityEngine.Android;

public class ButtonBehavior : MonoBehaviour
{
    public Sprite ButtonUp;
    public Sprite ButtonDown;
    private SpriteRenderer spriteRenderer;
    private Boolean IsPressed;
    public GameObject MyWall;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsPressed)
        {
            MyWall.SetActive(false);
        }
        else
        {
            MyWall.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has the tag "Box"
        if ((other.CompareTag("Box")) || (other.CompareTag("witchOW")) || (other.CompareTag("tankOW")) || (other.CompareTag("fighterOW")))
        {
            // Change the sprite to the new sprite
            spriteRenderer.sprite = ButtonDown;
            IsPressed = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Box")) || (other.CompareTag("witchOW")) || (other.CompareTag("tankOW")) || (other.CompareTag("fighterOW")))
        {
            // Change the sprite to the new sprite
            spriteRenderer.sprite = ButtonUp;
            IsPressed = false;
        }
    }
}

