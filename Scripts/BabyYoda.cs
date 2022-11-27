using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public class BabyYoda : MonoBehaviour
{
    private AIPath AIPath { get; set; } = null;
    private AIDestinationSetter AIDestinationSetter { get; set; } = null;
    private Player Player { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private Animator Animator { get; set; } = null;
    private CapsuleCollider2D BodyCollider { get; set; } = null;
    public Coroutine ShootCoroutine { get; private set; } = null;
    private int Damage { get; set; } = 420000;
    private bool Discovered { get; set; } = false;
    private bool Activated { get; set; } = false;
    private bool IsLookingRight { get; set; } = true;
    
    private void Awake()
    {
        if ((AIPath = this.gameObject.GetComponent<AIPath>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AIPath component."
            );
            Application.Quit(1);
        }
        
        if ((AIDestinationSetter = this.gameObject.GetComponent<AIDestinationSetter>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AIDestinationSetter component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError("ERROR: <BabyYoda> - Player game object was not found in game object hierarchy.");
            Application.Quit(1);
        }
        if ((Player = GameObject.Find("Player").GetComponent<Player>()) is null)
        {
            Debug.LogError("ERROR: <BabyYoda> - Player game object is missing Player component.");
            Application.Quit(1);
        }
 
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
            );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
            );
            Application.Quit(1);
        }
        
        if ((BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "CapsuleCollider2D component."
            );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        AIDestinationSetter.target = Player.transform;
        AIPath.canMove = false;
    }
    
    private void Update()
    {
        LookAtPlayer();
        
        float horizontalDistance = Player.transform.position.x - this.transform.position.x;
        float verticalDistance = Player.transform.position.y - this.transform.position.y;

        if (Math.Abs(horizontalDistance) < 10 &&
            Math.Abs(verticalDistance) < 3 &&
            !Discovered)
        {
            AudioManagement.Play("BabyYodaDiscoveryMusic", false);
            Discovered = true;
        }
        
        if (Math.Abs(horizontalDistance) < 3 &&
            Math.Abs(verticalDistance) < 3 &&
            !Activated && Input.GetKeyDown(KeyCode.E))
        {
            AudioManagement.PlayOneShot("CoinSpawnSound");
            AIPath.canMove = true;
            Activated = true;
        }
    }
    
    private void LookAtPlayer()
    {
        if ((Player.transform.position.x > (this.transform.position.x + BodyCollider.size.x) && !IsLookingRight)
            || (Player.transform.position.x < (this.transform.position.x - BodyCollider.size.x) && IsLookingRight))
        {
            transform.Rotate(0f, 180f, 0f);
            IsLookingRight = !IsLookingRight;
        }
    }
}
