using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyChaser : Enemy
{
    private Transform ObstacleDetectionPoint { get; set; } = null;
    private Coroutine MeleeAttackCoroutine { get; set; } = null;
    private Coroutine MeleeAttackStartDelayCoroutine { get; set; } = null;
    private CharacterMovementController CharacterMovementController { get; set; } = null;
    private Rigidbody2D Rigidbody2D { get; set; } = null;
    private Rigidbody2D PlayerRigidbody2D { get; set; } = null;
    private float HorizontalMove { get; set; } = 0f;
    private bool Jump { get; set; } = false;
    [field: SerializeField] protected float ViewRangeHorizontal { get; set; } = 11f;
    [field: SerializeField] protected float ViewRangeVertical { get; set; } = 4f;
    [field: SerializeField] protected int Damage { get; set; } = 0;
    [field: SerializeField] protected float Speed { get; set; } = 30f;

    protected new void Awake()
    {
        base.Awake();
        
        if ((ObstacleDetectionPoint = this.gameObject.transform.Find("ObstacleDetectionPoint")) == null)
        {
            Debug.LogError("<EnemyChaser> - Player game object is missing CharacterMovementController game object.");
        }
        
        if ((CharacterMovementController = this.gameObject.GetComponent<CharacterMovementController>()) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyChaser> - Player game object is missing CharacterMovementController component."
            );
            Application.Quit(1);
        }

        if ((Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyChaser> - " + this.gameObject.transform.name + " game object is missing " +
                "Rigidbody2D component."
            );
            Application.Quit(1);
        }

        if ((GameObject.Find("Player")) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyChaser> - Player game object was not " +
                "found in game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((PlayerRigidbody2D = GameObject.Find("Player").GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyChaser> - Player game object is missing Rigidbody2D component."
            );
            Application.Quit(1);
        }
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        HorizontalMove = 0 * Speed;
        
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        float horizontalDistance = Player.transform.position.x - this.transform.position.x;
        float verticalDistance = Player.transform.position.y - this.transform.position.y;
        
        if (Math.Abs(horizontalDistance) < ViewRangeHorizontal && 
            Math.Abs(horizontalDistance) > BodyCollider.size.x && 
            Math.Abs(verticalDistance) < ViewRangeVertical)
        {
            if (horizontalDistance > 0)
            {
                HorizontalMove = 1 * Speed;
            }
            else if (horizontalDistance < 0)
            {
                HorizontalMove = -1 * Speed;
            }

            // Jump if obstacle is in front of enemy.
            RaycastHit2D hit = Physics2D.Raycast(
                ObstacleDetectionPoint.position, ObstacleDetectionPoint.right, 0.1f
            );
            if (hit.collider is not null && !hit.collider.isTrigger &&
                (hit.transform.tag == "Tilemap" || hit.transform.tag == "OtherSolid") && 
                Math.Abs(Rigidbody2D.velocity.x) < 0.001f)
            {
                Jump = true;
            }

            // Jump if player jumps over enemy.
            if (Math.Abs(horizontalDistance) < 1.5f && verticalDistance < 2f)
            {
                if (PlayerRigidbody2D.velocity.y > 0.1f &&
                    ((Rigidbody2D.velocity.x < 0 && PlayerRigidbody2D.velocity.x > 0) ||
                     (Rigidbody2D.velocity.x > 0 && PlayerRigidbody2D.velocity.x < 0)))
                {
                    Jump = true;
                }
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

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MeleeAttackCoroutine is null && MeleeAttackStartDelayCoroutine is null)
            {
                MeleeAttackCoroutine = StartCoroutine(MeleeAttack());
            }
        }
    }
    
    protected void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MeleeAttackCoroutine is null && MeleeAttackStartDelayCoroutine is null)
            {
                MeleeAttackCoroutine = StartCoroutine(MeleeAttack());
            }
        }
    }
    
    protected void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MeleeAttackCoroutine is not null && MeleeAttackStartDelayCoroutine is null)
            {
                StopCoroutine(MeleeAttackCoroutine);
                MeleeAttackCoroutine = null;
                MeleeAttackStartDelayCoroutine = StartCoroutine(MeleeAttackStartDelay());
            }
        }
    }

    protected IEnumerator MeleeAttack()
    {
        while (true)
        {
            AudioManagement.PlayOneShot("MeleeHitSound");
            Player.TakeDamage(Damage);
            yield return new WaitForSeconds(0.3f);   
        }
    }
    
    private IEnumerator MeleeAttackStartDelay()
    {
        yield return new WaitForSeconds(0.3f);
        MeleeAttackStartDelayCoroutine = null;
    }
}
