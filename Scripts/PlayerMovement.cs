using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Jetpack Jetpack { get; private set; } = null;
    private Animator Animator { get; set; } = null;
    private CharacterMovementController CharacterMovementController { get; set; } = null;
    private Rigidbody2D Rigidbody2D { get; set; } = null;
    private float HorizontalMove { get; set; } = 0f;
    private float WalkSpeed { get; set; } = 30f;
    private bool Jump { get; set; } = false;
    
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

    private void Update()
    {
        HorizontalMove = Input.GetAxisRaw("Horizontal") * WalkSpeed;

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

    private void FixedUpdate()
    {
        Move();
        Jump = false;
    }

    private void Move()
    {
        CharacterMovementController.Move(HorizontalMove * Time.fixedDeltaTime, Jump);
    }
    
    public void SetFreeze(bool freeze)
    {
        if (freeze)
        {
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
