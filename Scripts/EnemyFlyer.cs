using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public abstract class EnemyFlyer : Enemy
{
    private AIPath AIPath { get; set; } = null;
    private AIDestinationSetter AIDestinationSetter { get; set; } = null;
    private Coroutine MeleeAttackCoroutine { get; set; } = null;
    private Coroutine MeleeAttackStartDelayCoroutine { get; set; } = null;
    [field: SerializeField] protected float ViewRangeHorizontal { get; set; } = 11f;
    [field: SerializeField] protected float ViewRangeVertical { get; set; } = 8f;
    protected int Damage { get; set; } = 0;
    
    protected new void Awake()
    {
        base.Awake();
        
        if ((AIPath = this.gameObject.GetComponent<AIPath>()) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyFlyer> - " + this.gameObject.transform.name + " game object is missing " +
                "AIPath component."
            );
            Application.Quit(1);
        }
        
        if ((AIDestinationSetter = this.gameObject.GetComponent<AIDestinationSetter>()) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyFlyer> - " + this.gameObject.transform.name + " game object is missing " +
                "AIDestinationSetter component."
            );
            Application.Quit(1);
        }
    }

    protected new void Start()
    {
        base.Start();

        AIDestinationSetter.target = Player.transform;
        AIPath.canMove = false;
    }

    
    protected new void Update()
    {
        base.Update();
        
        float horizontalDistance = Player.transform.position.x - this.transform.position.x;
        float verticalDistance = Player.transform.position.y - this.transform.position.y;

        if (Math.Abs(horizontalDistance) < ViewRangeHorizontal &&
            Math.Abs(verticalDistance) < ViewRangeVertical)
        {
            AIPath.canMove = true;
        }
        else
        {
            AIPath.canMove = false;
        }
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
