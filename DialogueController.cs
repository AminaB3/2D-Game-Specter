using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance{get; private set;}
    //Box for the dialogue
    public GameObject dialoguePanel;
    //Text for the dialogue and NPC/object name
    public TMP_Text dialogueText, nameText;

    //Container for choices
    public Transform choiceContainer;
    //Gameobject for buttons of choices
    public GameObject choiceButtonPrefab;
    //NPC that's speaking at the moment
    public NPC activeNPC;
    //NPC portrait image
    public Image portraitImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Only one instance of the game controller at a time
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        
    }
    //Show the dialogue box
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }
    //Set name text and sprite based on the sprite dialogue and portrait
    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }
    //Set dialogue text to be displayed
    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }
    //End dialogue so when player clicks on display, it terminates
    public void EndCurrentDialogue()
    {
        activeNPC.EndDialogue();
    }
    //Destroy choices
    public void destroyChoices()
    {
        foreach(Transform child in choiceContainer) Destroy(child.gameObject);
    }
    public GameObject CreateChoiceButton(string choiceText,UnityEngine.Events.UnityAction onClick )
    {
        //Copy of a button
        GameObject choiceButton = Instantiate (choiceButtonPrefab, choiceContainer);
        //Grab the text for the choice
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        //So the button registers the click from the mouse
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        //Return it
        return choiceButton;
    }
}

