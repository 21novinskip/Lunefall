using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIcons : MonoBehaviour
{
    public SpriteRenderer attackIcon;
    public Sprite[] atackSprites;
    public SpriteRenderer defenceIcon;
    public Sprite[] defenceSprites;
    public SpriteRenderer agilityIcon;
    public Sprite[] agilitySprites;
    public SpriteRenderer luckIcon;
    public Sprite[] luckSprites;
    // Start is called before the first frame update
    void Start()
    {
        attackIcon.sprite = atackSprites[0];
    }

    public void ChangeBuffIcon()
    {
        
    }
}
