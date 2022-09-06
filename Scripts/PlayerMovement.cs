using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Jetpack Jetpack { get; private set; }
    private Animator Animator { get; set; }
    private CharacterMovementController CharacterMovementController { get; set; }
    private Rigidbody2D Rigidbody2D { get; set; }
    private float HorizontalMove { get; set; }
    private float WalkSpeed { get; set; }
    private bool Jump { get; set; }
    public bool CanMove { get; set; }
    
    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMovement > - Player game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
		
        if ((Animator = GameObject.Find("Player").GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Player game object is missing Animator component."
                );
            Application.Quit(1);
        }
        
        if ((CharacterMovementController = GameObject.Find(
                "Player"
            ).GetComponent<CharacterMovementController>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Player game object is missing CharacterMovementController component."
                );
            Application.Quit(1);
        }
        
        if ((Rigidbody2D = GameObject.Find("Player").GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Player game object is missing Rigidbody2D component."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Player/Jetpack") is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMovement > - Player/Jetpack game object was not found in the game object " +
                "hierarchy."
                );
            Application.Quit(1);
        }
        if ((Jetpack = GameObject.Find("Player/Jetpack").GetComponent<Jetpack>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Player/Jetpack game object is missing Jetpack component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        Jump = false;
        CanMove = true;
        HorizontalMove = 0f;
        WalkSpeed = 30f;
    }

    private void Update()
    {
        if (CanMove)
        {
            HorizontalMove = Input.GetAxisRaw("Horizontal") * WalkSpeed;
            if (Rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
            {
                Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            }

            if (Input.GetButton("Jump"))
            {
                Jump = true;
            }

            if (Input.GetKeyDown("space"))
            {
                Jetpack.Activate();
                HorizontalMove *= 0.9f;
            }
            if (Input.GetKeyUp("space"))
            {
                Jetpack.Deactivate();
            }
        }
        else
        {
            HorizontalMove = 0;
            if (CharacterMovementController.IsGrounded && Rigidbody2D.bodyType != RigidbodyType2D.Static)
            {
                Rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }
    }
    
    private void FixedUpdate()
    {
        Move();
        Jump = false;
    }

    private void Move()
    {
        CharacterMovementController.Move(HorizontalMove * Time.fixedDeltaTime, Jump);
    }
}
