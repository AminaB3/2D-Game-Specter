using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    //Setting player movement speed, which can be edited later
    [SerializeField] private float moveSpeed = 3f;
    // Object of rigidbody 2d
    private Rigidbody2D rb;
    //Vector for character movement from keyboard input
    private Vector2 moveInput;
    //Animator object for movement animation of character
    private Animator animator;

    void Start()
    {
        //Get rigidbody component and animator component
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //If the game is paused during dialogue, character cannot move
        //Return so the rest of the lines don't execute
        if (Pause.isGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }
        //Movement is equal to input * movement speed
        rb.linearVelocity = moveInput * moveSpeed;
        //If moveInput is not zero, then character is moving
        bool moving = (moveInput != Vector2.zero);
        //If the character is not moving, default to the forward facing idle 
        //animation. Else, an animation will show depending on direction
        animator.SetBool("IsMoving", moving);

        //If moving
        if (moving)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }
    }
    //Input system calls whenever keys change. Stored in move input
    // and used during update
    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    }
}
