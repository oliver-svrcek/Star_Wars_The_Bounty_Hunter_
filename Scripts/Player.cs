using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData PlayerData { get; set; }
    private PlayerMovement PlayerMovement { get; set; }
    private PlayerWeapons PlayerWeapons { get; set; }
    private SpriteRenderer SpriteRenderer { get; set; }
    private Animator Animator { get; set; }
    private CapsuleCollider2D BodyCollider { get; set; }
    private Rigidbody2D Rigidbody2D { get; set; }
    private AudioManagement AudioManagement { get; set; }
    private PauseMenu PauseMenu { get; set; }
    private DeathMenu DeathMenu { get; set; }
    private BarManagement HealthBar { get; set; }
    private Coroutine HealCoroutine { get; set; }
    private Coroutine BleedCoroutine { get; set; }
    private int MaximumHealth { get; set; }
    private int CurrentHealth { get; set; }
    private float HealStartTime { get; set; }
    private int HealPoints { get; set; }
    private string DeathSound { get; set; }
    private bool CanHeal { get; set; }
    
    private void Awake()
    {
        if (ActivePlayer.PlayerData is null)
        {
            Debug.LogError("ERROR: <Player> - ActivePlayer.PlayerData is null.");
            Application.Quit(1);
        }

        if ((PlayerMovement = this.gameObject.GetComponent<PlayerMovement>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing PlayerMovement component.");
            Application.Quit(1);
        }
            
        if ((PlayerWeapons = this.gameObject.GetComponent<PlayerWeapons>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing PlayerWeapons component.");
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing Animator component.");
            Application.Quit(1);
        }

        if ((BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing CapsuleCollider2D component.");
            Application.Quit(1);
        }
        
        if ((Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing Rigidbody2D component.");
            Application.Quit(1);
        }
        
        if ((SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>()) is null)
        {
            Debug.LogError("ERROR: <Player> - Player game object is missing SpriteRenderer component.");
            Application.Quit(1);
        }

        if (GameObject.Find("Player/Audio/Other") is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Player/Audio/Other game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Player/Audio/Other"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Player/Audio/Other game object is missing AudioManagement component."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu") is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/PauseMenu game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((PauseMenu = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu"
                ).GetComponent<PauseMenu>()) is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/PauseMenu game object is missing PauseMenu " +
                "component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/UICanvas/DeathMenu") is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/DeathMenu game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((DeathMenu = GameObject.Find(
                "Interface/MainCamera/UICanvas/DeathMenu"
            ).GetComponent<DeathMenu>()) is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/DeathMenu game object is missing DeathMenu " +
                "component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/HUD/BarsWrapper/HealthBar/Slider"
            ) is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/HealthBar/Slider game " +
                "object was not found in game object hierarchy"
                );
            Application.Quit(1);
        }
        if ((HealthBar = GameObject.Find(
                "Interface/MainCamera/UICanvas/HUD/BarsWrapper/HealthBar/Slider"
            ).GetComponent<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Player> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/HealthBar/Slider game " +
                "object is missing BarManagement component."
                );
            Application.Quit(1);
        }

        PlayerData = ActivePlayer.PlayerData;
        MaximumHealth = 10000;
        CurrentHealth = MaximumHealth;
        HealStartTime = 4.5f;
        HealPoints = 10;
        HealCoroutine = null;
        BleedCoroutine = null;
        DeathSound = "PlayerDeathSound";
        CanHeal = true;
    }
    
    private void Start()
    {
        HealthBar.SetMaxValue(1f);
        HealthBar.SetValue(1f);
        HealthBar.SetGradient("Decreasing");
        
        ReloadGearValues();
        this.transform.position = new Vector3(PlayerData.PositionAxisX, PlayerData.PositionAxisY, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (DeathMenu.ActivateCoroutine is null)
            {
                if (PauseMenu.Paused)
                {
                    PauseMenu.Resume();
                }
                else
                {
                    PauseMenu.Pause();
                }
            }
        }
    }

    public void ReloadGearValues()
    {
        SetArmorValues();
        SetBlasterValues();
        SetJetpackValues();
        SetFlamethrowerValues();
    }

    private void SetArmorValues()
    {
        switch (PlayerData.ArmorLevel)
        {
            case 1:
                MaximumHealth = 10000;
                HealStartTime = 4.5f;
                HealPoints = 10;
                break;
            case 2:
                MaximumHealth = 13300;
                HealStartTime = 4f;
                HealPoints = 15;
                break;
            case 3:
                MaximumHealth = 16600;
                HealStartTime = 3.5f;
                HealPoints = 20;
                break;
            case 4:
                MaximumHealth = 20000;
                HealStartTime = 3f;
                HealPoints = 25;
                break;
        }
        
        ReloadHealth();
    }
    
    public void ReloadHealth()
    {
        if (HealCoroutine is not null)
        {
            StopCoroutine(HealCoroutine);
            HealCoroutine = null;
        }
    
        if (BleedCoroutine is not null)
        {
            StopCoroutine(BleedCoroutine);
            BleedCoroutine = null;
        }
    
        CurrentHealth = MaximumHealth;
        HealthBar.SetValue(1f);
        HealthBar.SetGradient("Decreasing");
    }

    private void SetBlasterValues()
    {
        switch (PlayerData.BlasterLevel)
        {
            case 1:
                PlayerWeapons.Blaster.BulletDamage = 2000;
                PlayerWeapons.Blaster.MaximumBlasterHeat = 10000;
                PlayerWeapons.Blaster.BlasterHeatPerShot = 2000;
                PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.2f;
                PlayerWeapons.Blaster.BlasterCoolingPower = 100;
                PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.6f;
                PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 60;
                break;
            case 2:
                PlayerWeapons.Blaster.BulletDamage = 3000;
                PlayerWeapons.Blaster.MaximumBlasterHeat = 10300;
                PlayerWeapons.Blaster.BlasterHeatPerShot = 1900;
                PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.19f;
                PlayerWeapons.Blaster.BlasterCoolingPower = 110;
                PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.53f;
                PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 63;
                break;
            case 3:
                PlayerWeapons.Blaster.BulletDamage = 4000;
                PlayerWeapons.Blaster.MaximumBlasterHeat = 10600;
                PlayerWeapons.Blaster.BlasterHeatPerShot = 1800;
                PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.18f;
                PlayerWeapons.Blaster.BlasterCoolingPower = 120;
                PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.46f;
                PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 66;
                break;
            case 4:
                PlayerWeapons.Blaster.BulletDamage = 5000;
                PlayerWeapons.Blaster.MaximumBlasterHeat = 11000;
                PlayerWeapons.Blaster.BlasterHeatPerShot = 1700;
                PlayerWeapons.Blaster.BlasterCoolingStartTime = 0.17f;
                PlayerWeapons.Blaster.BlasterCoolingPower = 130;
                PlayerWeapons.Blaster.BlasterOverheatCoolingStartTime = 0.4f;
                PlayerWeapons.Blaster.BlasterOverheatCoolingPower = 70;
                break;
        }
        
        PlayerWeapons.Blaster.Reload();
    }

    private void SetJetpackValues()
    {
        switch (PlayerData.JetpackLevel)
        {
            case 1:
                PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 70;
                PlayerMovement.Jetpack.JetpackFuelRechargePoints = 30;
                PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 1.2f;
                break;
            case 2:
                PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 65;
                PlayerMovement.Jetpack.JetpackFuelRechargePoints = 38;
                PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 1f;
                break;
            case 3:
                PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 60;
                PlayerMovement.Jetpack.JetpackFuelRechargePoints = 46;
                PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 0.8f;
                break;
            case 4:
                PlayerMovement.Jetpack.JetpackFuelConsumptionInitialPoints = 800;
                PlayerMovement.Jetpack.JetpackFuelConsumptionPoints = 55;
                PlayerMovement.Jetpack.JetpackFuelRechargePoints = 52;
                PlayerMovement.Jetpack.JetpackFuelRechargeStartTime = 0.5f;
                break;
        }
        
        PlayerMovement.Jetpack.Reload();
    }

    private void SetFlamethrowerValues()
    {
        switch (PlayerData.FlamethrowerLevel)
        {
            case 1:
                PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.25f;
                PlayerWeapons.Flamethrower.FlameRange = 2.75f;
                PlayerWeapons.Flamethrower.FlamethrowerDamage = 10000;
                PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 70;
                break;
            case 2:
                PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.5f;
                PlayerWeapons.Flamethrower.FlameRange = 5.5f;
                PlayerWeapons.Flamethrower.FlamethrowerDamage = 15000;
                PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 85;
                break;
            case 3:
                PlayerWeapons.Flamethrower.ParticleStartLifetime = 0.75f;
                PlayerWeapons.Flamethrower.FlameRange = 8.25f;
                PlayerWeapons.Flamethrower.FlamethrowerDamage = 20000;
                PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 100;
                break;
            case 4:
                PlayerWeapons.Flamethrower.ParticleStartLifetime = 1f;
                PlayerWeapons.Flamethrower.FlameRange = 11f;
                PlayerWeapons.Flamethrower.FlamethrowerDamage = 25000;
                PlayerWeapons.Flamethrower.FlamethrowerOverheatCoolingPoints = 115;
                break;
        }
        
        PlayerWeapons.Flamethrower.Reload();
    }

    private IEnumerator Heal()
    {
        if (DeathMenu.ActivateCoroutine is not null)
        {
            yield break;
        }
        
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
        if (DeathMenu.ActivateCoroutine is not null)
        {
            return;
        }
        
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
            AudioManagement.PlayOneShot(DeathSound);
            PlayerMovement.SetFreeze(true);
            PlayerMovement.enabled = false;
            PlayerWeapons.enabled = false;
            BodyCollider.enabled = false;
            Animator.enabled = false;
            if (BleedCoroutine is not null)
            {
                StopCoroutine(BleedCoroutine);
                BleedCoroutine = null;
            }
            SpriteRenderer.color = new Color32(255, 50, 50, 200);
            DeathMenu.Activate();
        }
        else if (CanHeal)
        {
            HealCoroutine = StartCoroutine(Heal());
        }
    }

    private IEnumerator Bleed()
    {
        if (DeathMenu.ActivateCoroutine is not null)
        {
            yield break;
        }
        
        SpriteRenderer.color = new Color32(255, 100, 100, 255);
        yield return new WaitForSeconds(0.2f);
        SpriteRenderer.color = new Color32(255, 255, 255, 255);
        BleedCoroutine = null;
    }
}
