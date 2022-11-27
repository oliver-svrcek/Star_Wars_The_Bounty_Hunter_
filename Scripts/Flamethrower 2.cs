using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private Animator Animator { get; set; }
    private AudioManagement AudioManagement { get; set; }
    private PlayerWeapons PlayerWeapons { get; set; }
    private PlayerMovement PlayerMovement { get; set; }
    private CharacterMovementController CharacterMovementController { get; set; }
    private BarManagement FlamethrowerHeatBar { get; set; }
    private ParticleSystem ParticleSystem { get; set; }
    private BoxCollider2D BoxCollider2D { get; set; }
    public Coroutine FlameCoroutine { get; private set; }
    public Coroutine CoolingCoroutine { get; private set; }
    public float ParticleStartLifetime { get; set; }
    public int MaximumFlamethrowerHeat { get; set; }
    public int CurrentFlamethrowerHeat  { get; set; }
    public int FlamethrowerHeatPoints { get; set; }
    public int FlamethrowerOverheatCoolingPoints { get; set; }
    public float FlameRange { get; set; }
    public int FlamethrowerDamage { get; set; }
    private List<Enemy> EnemiesInRange { get; set; }

    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
		
        if ((Animator = GameObject.Find("Player").GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player game object is missing Animator component."
                );
            Application.Quit(1);
        }
        
        if ((PlayerWeapons = GameObject.Find("Player").GetComponent<PlayerWeapons>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player game object is missing PlayerWeapons component."
                );
            Application.Quit(1);
        }
        
        if ((PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player game object is missing PlayerMovement component."
                );
            Application.Quit(1);
        }
        
        if ((CharacterMovementController = GameObject.Find(
                "Player"
                ).GetComponent<CharacterMovementController>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player game object is missing " +
                "CharacterMovementController component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Audio/Flamethrower") is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player/Audio/Flamethrower game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Player/Audio/Flamethrower"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Player/Audio/Flamethrower game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/HUD/BarsWrapper/FlamethrowerBar/Slider"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/FlamethrowerBar/Slider" +
                " game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FlamethrowerHeatBar = GameObject.Find(
                "Interface/MainCamera/UICanvas/HUD/BarsWrapper/FlamethrowerBar/Slider"
                ).GetComponent<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - UICanvas/MainCamera/UICanvas/HUD/BarsWrapper/" +
                "FlamethrowerBar/Slider game object is missing BarManagement component."
                );
            Application.Quit(1);
        }
        
        if ((ParticleSystem = this.gameObject.GetComponent<ParticleSystem>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - " + this.gameObject.transform.name + " game object is missing " +
                "ParticleSystem component."
                );
            Application.Quit(1);
        }
        
        if ((BoxCollider2D = this.gameObject.GetComponent<BoxCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <Flamethrower> - " + this.gameObject.transform.name + " game object is missing " +
                "BoxCollider2D component."
                );
            Application.Quit(1);
        }
        
        ParticleStartLifetime = 0.25f;
        MaximumFlamethrowerHeat = 100000;
        CurrentFlamethrowerHeat = 0;
        FlamethrowerHeatPoints = 500;
        FlamethrowerOverheatCoolingPoints = 50;
        FlameRange = 2.75f;
        FlamethrowerDamage = 11000;

        FlameCoroutine = null;
        CoolingCoroutine = null;
        EnemiesInRange = new List<Enemy>();
    }

    private void Start()
    {
        AudioManagement.Stop();
        ParticleSystem.MainModule particleSystemMain = ParticleSystem.main;
        particleSystemMain.startLifetime = ParticleStartLifetime;
        ParticleSystem.Stop();
        BoxCollider2D.offset = new Vector2(0.1f, 0);
        BoxCollider2D.size = new Vector2(0.2f, 1.5f);
        BoxCollider2D.enabled = false;
        FlamethrowerHeatBar.SetMaxValue(1f);
        FlamethrowerHeatBar.SetValue(0f);
        FlamethrowerHeatBar.SetGradient("Increasing");
    }

    public void Reload()
    {
        if (FlameCoroutine is not null)
        {
            Animator.SetBool("IsUsingFlamethrower", false);
            StopCoroutine(FlameCoroutine);
            FlameCoroutine = null;
        }

        if (CoolingCoroutine is not null)
        {
            StopCoroutine(CoolingCoroutine);
            CoolingCoroutine = null;
        }

        ParticleSystem.MainModule particleSystemMain = ParticleSystem.main;
        particleSystemMain.startLifetime = ParticleStartLifetime;
        CurrentFlamethrowerHeat = 0;
        FlamethrowerHeatBar.SetValue(0f);
        FlamethrowerHeatBar.SetGradient("Increasing");
    }

    public void Activate()
    {
        if (FlameCoroutine is null && CoolingCoroutine is null && CharacterMovementController.IsGrounded)
        {
            FlameCoroutine = StartCoroutine(Flame());
        }
        else
        {
            AudioManagement.PlayOneShot("AbilityNotAvailableSound");
        }
    }
    
    private IEnumerator Flame()
    {
        Animator.SetBool("IsUsingFlamethrower", true);
        
        PlayerMovement.SetFreeze(true);
        PlayerMovement.enabled = false;
        PlayerWeapons.enabled = false;
        ParticleSystem.Play();
        BoxCollider2D.enabled = true;
        Coroutine increaseRangeCoroutine = StartCoroutine(IncreaseRange());
        Coroutine damageEnemiesCoroutine = StartCoroutine(DamageEnemies());

        AudioManagement.Play("FlameSoundLong", true);

        while (CurrentFlamethrowerHeat < MaximumFlamethrowerHeat)
        {
            FlamethrowerHeatBar.SetValue((float) CurrentFlamethrowerHeat / MaximumFlamethrowerHeat);
            CurrentFlamethrowerHeat += FlamethrowerHeatPoints;
            yield return new WaitForSeconds(0.01f);
        }

        CurrentFlamethrowerHeat = MaximumFlamethrowerHeat;
        FlamethrowerHeatBar.SetValue((float) CurrentFlamethrowerHeat / MaximumFlamethrowerHeat);
        
        Animator.SetBool("IsUsingFlamethrower", false);
        
        ParticleSystem.Stop();
        StopCoroutine(increaseRangeCoroutine);
        AudioManagement.Stop();
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        StopCoroutine(damageEnemiesCoroutine);
        BoxCollider2D.offset = new Vector2(0.1f, 0);
        BoxCollider2D.size = new Vector2(0.2f, 1.5f);
        BoxCollider2D.enabled = false;
        EnemiesInRange = new List<Enemy>();
        CoolingCoroutine = StartCoroutine(Cooling());
        PlayerWeapons.enabled = true;
        PlayerMovement.enabled = true;
        PlayerMovement.SetFreeze(false);
        FlameCoroutine = null;
    }
    
    private IEnumerator Cooling()
    {
        FlamethrowerHeatBar.SetGradient("Recharging");
        FlamethrowerHeatBar.SetValue(1f);
         
        while (CurrentFlamethrowerHeat > FlamethrowerOverheatCoolingPoints)
        {
            yield return new WaitForSeconds(0.01f);
            CurrentFlamethrowerHeat -= FlamethrowerOverheatCoolingPoints;
            FlamethrowerHeatBar.SetValue((float) CurrentFlamethrowerHeat / MaximumFlamethrowerHeat);
        }
         
        AudioManagement.PlayOneShot("AbilityRechargedSound");
        CurrentFlamethrowerHeat = 0;
        FlamethrowerHeatBar.SetGradient("Increasing");
        FlamethrowerHeatBar.SetValue(0f);
        CoolingCoroutine = null;
    }
    
    private IEnumerator IncreaseRange()
    {
        while (BoxCollider2D.size.x < FlameRange)
        {
            BoxCollider2D.size = new Vector2(BoxCollider2D.size.x + 0.14f, BoxCollider2D.size.y);
            BoxCollider2D.offset = new Vector2(BoxCollider2D.size.x / 2, BoxCollider2D.offset.y);
            yield return new WaitForSeconds(0.01f);
        }
        
        BoxCollider2D.size = new Vector2(FlameRange, BoxCollider2D.size.y);
        BoxCollider2D.offset = new Vector2(BoxCollider2D.size.x / 2, BoxCollider2D.offset.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger || other.CompareTag("Player"))
        {
            return;
        }
        
        Enemy enemy;
        if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
        {
            if (!EnemiesInRange.Contains(enemy))
            {
                EnemiesInRange.Add(enemy);
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.isTrigger || other.CompareTag("Player"))
        {
            return;
        }
        
        Enemy enemy;
        if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
        {
            if (!EnemiesInRange.Contains(enemy))
            {
                EnemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger || other.CompareTag("Player"))
        {
            return;
        }
        
        Enemy enemy;
        if ((enemy = other.gameObject.GetComponentInParent<Enemy>()) is not null)
        {
            EnemiesInRange.Remove(enemy);
        }
    }

    private IEnumerator DamageEnemies()
    {
        while (true)
        {
            foreach (Enemy enemy in EnemiesInRange.ToArray())
            {
                if (enemy != null)
                {
                    AudioManagement.PlayOneShot("HitmarkerSound");
                    enemy.TakeDamage(FlamethrowerDamage);
                }
            }
        
            yield return new WaitForSeconds(0.3f);
        }
    }
}
