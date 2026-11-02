using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Track closest interactable to player
    
    //Interactable object or NPC closest to player
    private IInteractable interactableInRange = null;
    //Little pop-up that shows something is interactable
    public GameObject interactionIcon;
    void Start()
    {
        //Defaults to inactive
        interactionIcon.SetActive(false);
    }
    //Using unity input system for input action, when player presses
    // e to interact 
    public void OnInteract(InputAction.CallbackContext context)
    {
        //If e is pressed and the interactable is within range, interact
        if (context.performed) interactableInRange?.Interact();
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision){
        //Range for interaction was set. If within range of an interactable object with IInteractable script and
        //set interactableInRange to the interactable and set to true
        //&CanInteract is checking if we aren't mid dialogue already
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract()){
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
        }
    }
    //If we leave the space that triggers the interaction icon, set
    //the interactableInRange to null and interaction icon to false
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange){
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }

}