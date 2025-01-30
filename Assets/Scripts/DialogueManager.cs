using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
 
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
 
    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;
 
    public Animator animator;
    public GameObject Witch;
    public GameObject Tank;
    public GameObject Fighter;
    private GameObject Speaker;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
    }
    public void StartDialogue(Dialogue dialogue, GameObject passed_speaker)
    {
        Speaker = passed_speaker;
        isDialogueActive = true;

        //Witch.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.DIALOGUE;
        //Tank.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.DIALOGUE;
        //Fighter.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.DIALOGUE;
 
        animator.Play("show");
 
        lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine();
    }
    public IEnumerator StartBattlePopup(Sprite faceball , string comboName , string description)
    {
        isDialogueActive = true;
        //dialogueArea.fontSize = 130;
        //animator.SetTrigger("ShowB");
        lines.Clear();

        characterIcon.sprite = faceball;
        characterName.text = comboName;
        dialogueArea.text = description;
        yield return new WaitForSeconds(1.5f);
        //animator.SetTrigger("HideB");
        //dialogueArea.fontSize = 100;
        yield return null;
    }
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        StartCoroutine(DMNext());
    }
 
    public void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");
        //Witch.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.FREE;
        //Fighter.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.FREE;
        //Tank.GetComponent<PlayerMovement>().PlayerState = PlayerMovement.PLAYERSTATES.FREE;
        if (Speaker.GetComponent<DialogueTrigger>().KillOnCompletion == true)
        {
            Speaker.SetActive(false);
        }
    }

    IEnumerator DMNext()
    {
        while(isDialogueActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DisplayNextDialogueLine();
                yield break;
            }
            yield return null;
        }
        yield return null;
    }
}