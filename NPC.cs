
using System.Collections;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
   //Object of NPCDialogue with all of the data for the NPC
    public NPCDialogue dialogueData;
    //Index of the dialogue
    private int dialogueIndex;
    //Boolean for if isTyping and if the dialogue is activbe
    private bool isTyping, isDialogueActive;
    //Object of the dialogue Controller for the UI of the NPC
    private DialogueController dialogueUI;


    //Object of the Character's/Interactable object's audio source
    private AudioSource audioSource;

    private void Start()
    {
        //Has control over the Dialogue controller instance 
        dialogueUI = DialogueController.Instance;
        //Get the audio source component we added to the sprite
        //Play on awake is left unchecked so it will only play when we interact with the NPC
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanInteract()
    {
        //Sends back that dialogue is not active, therefore we can
        //initiate an interaction and begin dialogue with NPC
        return !isDialogueActive;
    }

    public void Interact()
    {
        //If the dialogue is active, continue, else start dialogue fresh
        if(isDialogueActive) NextLine();
        else StartDialogue();
    }

    private void SyncInvestigativeState()
    {
        if(dialogueIndex >= 12) dialogueIndex = dialogueData.investigationInProgressIndex;
        else dialogueIndex = 0;
    }
    void StartDialogue()
    {
        //Sync with investigative data for Detective, if we already spoke, it starts back at line 12, 
        //else begins at line 0
        SyncInvestigativeState();

        //Dialogue started
        isDialogueActive = true;
        //The current NPC is the active one
        dialogueUI.activeNPC = this;
        //Set the portrait and name to the NPC's information
        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        //Display the UI
        dialogueUI.ShowDialogueUI(true);
        //Make it so that the player can't move around while the NPC is speaking
        Pause.SetPause(true);
        //Call displaycurrentlines, which calls TypeLine
        DisplayCurrentLines();
    }
    void NextLine()
    {
        //If the player types e, begins next dialogue line
        if (isTyping)
        {
            //Pause execution
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }
        //Clears old buttons, this would be handy in the case that
        //players have multiple choices
        dialogueUI.destroyChoices();
        //Out of range guard and check to see if that dialogue line is ticked to end in inspector
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }
        //For each choice event, if the index for the choice matches the current index
        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                //Display the choices for choice event
                DisplayChoices(dialogueChoice);
                //Return so we don't just continue to the next dialogue index
                return;
            }
        }
        //Otherwise if there is no choice event and we're not
        // at the end of dialogue, display current lines
        if(++dialogueIndex < dialogueData.dialogueLines.Length) DisplayCurrentLines();
        else EndDialogue();

    }

    IEnumerator TypeLine()
    {
        //Blank the text box
        isTyping = true;
        dialogueUI.SetDialogueText("");

        //For each loop to loop through the dialogue strings so
        // it prints letter by letter in the dialogue box
        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        //If we don't go outside of the last dialogue index and we autoprogress
        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            //Wait.a few seconds based on decided delay
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            //Call NextLine()
            NextLine();
        }
    }
    void DisplayChoices(DialogueChoice choice)
    {
        //For each choice, a clickable choice button is created that
        //goes to the next index it was assigned as a response
        for(int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => chooseOption(nextIndex));
        }
    }
    //Displays the index that was assigned as the response to the choice
    void chooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.destroyChoices();
        DisplayCurrentLines();
    }
    void DisplayCurrentLines()
    {
        //Text stops typing, and next line starts
        StopAllCoroutines();
        StartCoroutine(TypeLine());
        audioSource.PlayOneShot(dialogueData.voiceSound);
    }

//Ends the dialogue!
public void EndDialogue()
    {
        dialogueUI.activeNPC = null;
        StopAllCoroutines();
        isTyping = false;
        dialogueUI.destroyChoices();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
         dialogueUI.ShowDialogueUI(false);
        Pause.SetPause(false);
    }
}
