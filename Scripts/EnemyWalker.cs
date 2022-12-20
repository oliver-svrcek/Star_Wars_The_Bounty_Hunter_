using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyWalker : Enemy
{
    private Coroutine MeleeAttackCoroutine { get; set; } = null;
    private Coroutine MeleeAttackStartDelayCoroutine { get; set; } = null;
    private Vector3 PatrolStartPosition { get; set; } = new Vector3();
    private Vector3 PatrolEndPosition { get; set; } = new Vector3();
    private Vector3 PatrolTargetPosition { get; set; } = new Vector3();
    private CharacterMovementController CharacterMovementController { get; set; } = null;
    private float HorizontalMove { get; set; } = 0f;
    private bool Jump { get; set; } = false;
    [field: SerializeField] protected int Damage { get; set; } = 0;
    [field: SerializeField] protected float Speed { get; set; } = 30f;
    
    protected new void Awake()
    {
        base.Awake();
        
        if ((CharacterMovementController = this.gameObject.GetComponent<CharacterMovementController>()) is null) 
        {
            Debug.LogError(
                 "ERROR: <EnemyChaser> - Player game object is missing CharacterMovementController component."
                 );
            Application.Quit(1);
        }
        
        if (this.gameObject.transform.Find("PatrolEndPosition") is null)
        {
            Debug.LogError(
                "ERROR: <EnemyWalker> - " + this.gameObject.transform.name + "/PatrolEndPosition game object" +
                "was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
    }

    protected new void Start()
    {
        base.Start();
        
        PatrolStartPosition = this.gameObject.transform.position;
        PatrolEndPosition = this.gameObject.transform.Find("PatrolEndPosition").position;
        PatrolTargetPosition = PatrolEndPosition;
    }

    protected new void Update()
    { 
        HorizontalMove = 0 * Speed;
        
        Patrol();
    }

    private void Patrol()
    {
        if (PatrolTargetPosition.Equals(PatrolEndPosition))
        {
            HorizontalMove = 1 * Speed;
        }
        else
        {
            HorizontalMove = -1 * Speed;
        }
        
        if (Math.Abs(this.gameObject.transform.position.x - PatrolTargetPosition.x) < 0.1f)
        {
            if (PatrolTargetPosition.Equals(PatrolStartPosition))
            {
                PatrolTargetPosition = PatrolEndPosition;
            }
            else
            {
                PatrolTargetPosition = PatrolStartPosition;
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
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MeleeAttackCoroutine is null && MeleeAttackStartDelayCoroutine is null)
            {
                MeleeAttackCoroutine = StartCoroutine(MeleeAttack());
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MeleeAttackCoroutine is null && MeleeAttackStartDelayCoroutine is null)
            {
                MeleeAttackCoroutine = StartCoroutine(MeleeAttack());
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
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

    private IEnumerator MeleeAttack()
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