using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Coroutine MoveStartDelayCoroutine { get; set; } = null;
    private Vector3 MoveStartPosition { get; set; } = new Vector3();
    private Vector3 MoveEndPosition { get; set; } = new Vector3();
    private Vector3 MoveTargetPosition { get; set; } = new Vector3();
    [field: SerializeField] private float Speed { get; set; } = 4f;
    [field: SerializeField] private float MoveStartDelayTime { get; set; } = 0f;

    protected void Awake()
    {
        if (this.gameObject.transform.Find("MoveEndPosition") is null)
        {
            Debug.LogError(
                "ERROR: <MovingPlatform> - " + this.gameObject.transform.name + "/MoveEndPosition game " +
                "object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        MoveStartPosition = this.gameObject.transform.position;
        MoveEndPosition = this.gameObject.transform.Find("MoveEndPosition").position;
        MoveTargetPosition = MoveEndPosition;
    }

    private void Start()
    {
        MoveStartDelayCoroutine = StartCoroutine(MoveStartDelay());
    }

    private void Update()
    {   
        if (MoveStartDelayCoroutine is null)
        {
            Move();
        }
    }

    private void Move()
    {
        if (Vector2.Distance(transform.position, MoveTargetPosition) < 0.01f)
        {
            if (MoveTargetPosition.Equals(MoveStartPosition))
            {
                MoveTargetPosition = MoveEndPosition;
            }
            else
            {
                MoveTargetPosition = MoveStartPosition;
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position, MoveTargetPosition, Speed * Time.deltaTime
            );
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponentInParent<Player>();

            if (player is not null)
            {
                player.transform.parent = this.transform;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision) 
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
             Player player = collision.GetComponentInParent<Player>();
     
             if (player is not null)
             {
                 player.transform.parent = null;
             }
        }
    }
    
    private IEnumerator MoveStartDelay()
    {
        yield return new WaitForSeconds(MoveStartDelayTime);
        MoveStartDelayCoroutine = null;
    }
}
