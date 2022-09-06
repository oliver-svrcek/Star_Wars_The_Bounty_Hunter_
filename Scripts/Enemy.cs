using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Player Player { get; set; }
    protected AudioManagement AudioManagement { get; set; }
    private SpriteRenderer SpriteRenderer { get; set; }
    private CapsuleCollider2D BodyCollider { get; set; }
    private GameObject HealthBarGameObject { get; set; }
    private BarManagement HealthBar { get; set; }
    private TextMeshProUGUI BossName { get; set; }
    private Coroutine HealCoroutine { get; set; }
    private Coroutine BleedCoroutine { get; set; }
    protected int MaximumHealth { get; set; }
    protected int CurrentHealth { get; set; }
    protected float HealStartTime { get; set; }
    protected int HealPoints { get; set; }
    protected string DeathSound { get; set; }
    protected bool CanHeal { get; set; }
    private bool IsLookingRight { get; set; }

    protected void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError("ERROR: <Enemy> - Player game object was not found in game object hierarchy.");
            Application.Quit(1);
        }
        if ((Player = GameObject.Find("Player").GetComponent<Player>()) is null)
        {
            Debug.LogError("ERROR: <Enemy> - Player game object is missing Player component.");
            Application.Quit(1);
        }
 
        if ((AudioManagement = this.gameObject.GetComponentInChildren<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Enemy> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>()) is null)
        {
            Debug.LogError(
                "ERROR: <Enemy> - " + this.gameObject.transform.name + " game object is missing " +
                "SpriteRenderer component."
                );
            Application.Quit(1);
        }
        
        if ((BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <Enemy> - " + this.gameObject.transform.name + " game object is missing " +
                "CapsuleCollider2D component."
                );
            Application.Quit(1);
        }

        if (this.gameObject.CompareTag("Enemy") 
            && (HealthBar = this.gameObject.GetComponentInChildren<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Enemy> - " + this.gameObject.transform.name + " game object is missing " +
                "BarManagement component."
                );
            Application.Quit(1);
        }
        
        if (this.gameObject.CompareTag("Boss"))
        {
            if (GameObject.Find("Interface/MainCamera/UICanvas/HUD/BossHealthBar") is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar game object was not " +
                    "found in game object hierarchy."
                    );
                Application.Quit(1);
            }
            if ((HealthBarGameObject = GameObject.Find("Interface/MainCamera/UICanvas/HUD/BossHealthBar")) is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar game object was not " +
                    "found in game object hierarchy."
                    );
                Application.Quit(1);
            }
            HealthBarGameObject.SetActive(true);
            
            if (GameObject.Find("Interface/MainCamera/UICanvas/HUD/BossHealthBar/Slider") is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar/Slider game object was " +
                    "not found in game object hierarchy."
                    );
                Application.Quit(1);
            }
            if ((HealthBar = GameObject.Find(
                    "Interface/MainCamera/UICanvas/HUD/BossHealthBar/Slider"
                ).GetComponent<BarManagement>()) is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar/Slider game object" +
                    "is missing BarManagement component."
                    );
                Application.Quit(1);
            }
        
            if (GameObject.Find("Interface/MainCamera/UICanvas/HUD/BossHealthBar/BossName") is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar/BossName game object " +
                    "was not found in game object hierarchy."
                    );
                Application.Quit(1);
            }
            if ((BossName = GameObject.Find(
                    "Interface/MainCamera/UICanvas/HUD/BossHealthBar/BossName"
                ).GetComponent<TextMeshProUGUI>()) is null)
            {
                Debug.LogError(
                    "ERROR: <Enemy> - Interface/MainCamera/UICanvas/HUD/BossHealthBar/BossName game object" +
                    "is missing TextMeshProUGUI component."
                    );
                Application.Quit(1);
            }
        }
    }

    protected void Start()
    {
        HealCoroutine = null;
        BleedCoroutine = null;
        DeathSound = "GenericDeathSound";
        CanHeal = false;
        IsLookingRight = true;
        HealthBar.SetMaxValue(1f);
        HealthBar.SetValue(1f);
        HealthBar.SetGradient("EnemyHealth");

        if (this.gameObject.CompareTag("Boss"))
        {
            BossName.text = Regex.Replace(this.gameObject.name, "(\\B[A-Z])", " $1");
        }
    }

    protected void Update()
    {
        LookAtPlayer();
    }

    protected void LookAtPlayer()
    {
        if ((Player.transform.position.x > (this.transform.position.x + BodyCollider.size.x) && !IsLookingRight)
            || (Player.transform.position.x < (this.transform.position.x - BodyCollider.size.x) && IsLookingRight))
        {
            transform.Rotate(0f, 180f, 0f);
            IsLookingRight = !IsLookingRight;
        }
    }
    
    protected IEnumerator Heal()
    {
        yield return new WaitForSeconds(HealStartTime);

        while (CurrentHealth < (MaximumHealth - HealPoints))
        {
            CurrentHealth += HealPoints;
            HealthBar.SetValue((float) CurrentHealth / MaximumHealth);
            yield return new WaitForSeconds(0.01f);
        }

        CurrentHealth = MaximumHealth;
        HealthBar.SetValue((float) CurrentHealth / MaximumHealth);
        HealCoroutine = null;
    }

    public void TakeDamage(int damage)
    {
        if (HealCoroutine is not null)
        {
            StopCoroutine(HealCoroutine);
        }
        if (BleedCoroutine is not null)
        {
            StopCoroutine(BleedCoroutine);
        }
        BleedCoroutine = StartCoroutine(Bleed());
        
        CurrentHealth -= damage;
        HealthBar.SetValue(((float) CurrentHealth / MaximumHealth));

        if (CurrentHealth <= 0)
        {
            AudioManagement.PlayClipAtPoint(DeathSound, this.gameObject.transform.position);
            AudioManagement.RemoveFromMainAudioManagement();

            if (this.gameObject.CompareTag("Boss"))
            {
                HealthBarGameObject.SetActive(false);
            }
            
            Destroy(this.gameObject);
        }
        else if (CanHeal)
        {
            HealCoroutine = StartCoroutine(Heal());
        }
    }
    
    private IEnumerator Bleed()
    {
        SpriteRenderer.color = new Color32(255, 100, 100, 255);
        yield return new WaitForSeconds(0.2f);
        SpriteRenderer.color = new Color32(255, 255, 255, 255);
        BleedCoroutine = null;
    }
}
