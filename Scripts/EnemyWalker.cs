using System.Collections;
using UnityEngine;

public abstract class EnemyWalker : Enemy
{
    protected Coroutine MeleeAttackCoroutine { get; set; }
    protected Coroutine MeleeAttackStartDelayCoroutine { get; set; }
    protected Vector3 PatrolStartPosition { get; set; }
    protected Vector3 PatrolEndPosition { get; set; }
    protected Vector3 PatrolTargetPosition { get; set; }
    protected int Damage { get; set; }
    protected float Speed { get; set; }
    
    protected new void Awake()
    {
        base.Awake();
        
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
        
        MeleeAttackCoroutine = null;
        PatrolStartPosition = this.gameObject.transform.position;
        PatrolEndPosition = this.gameObject.transform.Find("PatrolEndPosition").position;
        PatrolTargetPosition = PatrolEndPosition;
    }
    
    protected new void Update()
    {
        Move();
    }

    protected void Move()
    {
        if (Vector2.Distance(transform.position, PatrolTargetPosition) < 0.01f)
        {
            transform.Rotate(0f, 180f, 0f);
            if (PatrolTargetPosition.Equals(PatrolStartPosition))
            {
                PatrolTargetPosition = PatrolEndPosition;
            }
            else
            {
                PatrolTargetPosition = PatrolStartPosition;
            }
        }
        
        transform.position = Vector2.MoveTowards(
            transform.position, 
            PatrolTargetPosition, 
            Speed * Time.deltaTime
            );
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
