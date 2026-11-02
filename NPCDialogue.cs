using UnityEngine;

//Making the NPC Dialogue into a scriptable object so each NPC can have 
//their own lines, portrait, etc
public class NPCDialogue : ScriptableObject 
{
    //Name of the NPC
    public string npcName;
    //Portrait of the NPC
    public Sprite npcPortrait;
    //String array of their dialogue
    public string[] dialogueLines;
    //Bool array of what lines autoprogress and which don't
    public bool[] autoProgressLines;
    //Bool of what lines end the interaction dialogue
    public bool [] endDialogueLines;
    //How long until it autoprogresses until next line
    public float autoProgressDelay = 1.5f;
    //Speed of the dialogue being typed out in each line
    public float typingSpeed = 1f;
    //Voice clip for the NPC
    public AudioClip voiceSound;
    //Groups of choices, since each NPC can prompt a conversation
    //Where the character could make many choices
    public DialogueChoice[] choices;

    //Reiterates question while we investigate
    public int investigationInProgressIndex; 
    

}

//Allow the custom dialogue class to be show to Inspector
[System.Serializable]

public class DialogueChoice
{
    //Dialogue of the index that triggers the choices
    public int dialogueIndex;
    //String array of choices for player to choose from
    public string[] choices;
    //The next dialogue index after the player chooses so the Detective can respond
    public int[] nextDialogueIndexes;
}
