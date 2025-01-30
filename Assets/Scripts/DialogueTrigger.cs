using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
[System.Serializable]


public class DialogueCharacter
{
    public string name;
    public Sprite icon;

}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool waiting = false;
    public bool KillOnCompletion;
 
    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, gameObject);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pmov = collision.gameObject.GetComponent<PlayerMovement>();
        if(((collision.tag == "fighterOW") || (collision.tag == "tankOW") || (collision.tag == "witchOW")) && pmov.TrainPosition == PlayerMovement.TRAINPOSITIONS.FIRST)
        {
            Debug.Log("TRIGGER ENTERED.");
            waiting = true;
            TriggerDialogue();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        waiting = false;
        DialogueManager.Instance.EndDialogue();
        if (KillOnCompletion == true)
        {
            gameObject.SetActive(false);
        }
    }
    //for the rat that blocks u
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pmov = collision.gameObject.GetComponent<PlayerMovement>();
        if(((collision.gameObject.tag == "fighterOW") || (collision.gameObject.tag == "tankOW") || (collision.gameObject.tag == "witchOW")) && pmov.TrainPosition == PlayerMovement.TRAINPOSITIONS.FIRST)
        {
            Debug.Log("TRIGGER ENTERED.");
            waiting = true;
            TriggerDialogue();
        }
    }
    IEnumerator WaitForConfirm()
    {
        yield return null;
    }
}