using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D { get; set; }
    private AudioManagement AudioManagement { get; set; }
    
    private string ParentTag { get; set; }
    private Vector3 ParentPosition { get; set; }
    private int Damage { get; set; }
    private float Speed { get; set; }
    private float SelfDestructDistance { get; set; }
    private bool Initialized { get; set; }

    private void Awake()
    {
        if ((Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <Bullet> - " + this.gameObject.transform.name + " game object is missing Rigidbody2D " +
                "component."
                );
            Application.Quit(1);
        }
        
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Bullet> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        ParentTag = "";
        ParentPosition = new Vector3();
        Damage = 0;
        Speed = 20f;
        SelfDestructDistance = 30;
        Initialized = false;

    }

    private void Start()    
    {
        Move();
    }
 
    public void Initialize(GameObject parent, int damage, float speed)
    {
        if (!Initialized)
        {
            Damage = damage;
            Speed = speed;
            ParentTag = parent.gameObject.tag;
            ParentPosition = parent.transform.position;
        }
        else
        {
            Debug.LogWarning(
                "WARNING: <Bullet> - " + this.gameObject.transform.name + " is already initialized."
                );
        }
    }
    
    private void Update()    
    {
        if (Vector3.Distance(this.transform.position, ParentPosition) > SelfDestructDistance)
        {
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        Rigidbody2D.velocity = this.gameObject.transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OtherSolid"))
        {
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
            return;
        }
        
        if (other.isTrigger)
        {
            return;
        }
        
        if (ParentTag == "Player")
        {
            if (other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            Enemy enemy;
            if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
            {
                AudioManagement.PlayClipAtPoint
                ("HitmarkerSound", 
                    this.gameObject.transform.position
                );
                enemy.TakeDamage(Damage);
            }
        }
        else
        {
            if (other.gameObject.GetComponentInParent<Enemy>() is not null)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("Player"))
            {
                Player player;
                if ((player = other.gameObject.GetComponentInParent<Player>()) is not null)
                {
                    AudioManagement.PlayClipAtPoint
                        ("HitmarkerSound", 
                            this.gameObject.transform.position
                            );
                    player.TakeDamage(Damage);
                }
            }
        }

        AudioManagement.RemoveFromMainAudioManagement();
        Destroy(this.gameObject);
    }
}