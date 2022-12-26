using System.Collections;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    private Animator Animator { get; set; } = null;
    private AudioManagement AudioManagement { get; set; }  = null;
    private BarManagement BlasterHeatBar { get; set; } = null;
    private GameObject BulletPrefab { get; set; } = null;
    public Coroutine ShootCoroutine { get; private set; } = null;
    public Coroutine CoolingCoroutine { get; private set; } = null;
    public Coroutine OverheatCoroutine { get; private set; } = null;
    public int MaximumBlasterHeat { get; set; } = 10000;
    private int CurrentBlasterHeat { get; set; } = 0;
    public int BlasterHeatPerShot { get; set; } = 2000;
    public float BlasterCoolingStartTime { get; set; } = 0.2f;
    public int BlasterCoolingPower { get; set; } = 100;
    public float BlasterOverheatCoolingStartTime { get; set; } = 0.6f;
    public int BlasterOverheatCoolingPower { get; set; } = 60;
    public int BulletDamage { get; set; } = 2000;
    private float BulletSpeed { get; set; } = 20f;
    private string BulletSound { get; set; } = "PlayerBlasterShotSound";

    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError("ERROR: <Blaster> - Player game object was not found in the game object hierarchy.");
            Application.Quit(1);
        }
		
        if ((Animator = GameObject.Find("Player").GetComponent<Animator>()) is null)
        {
            Debug.LogError("ERROR: <Blaster> - Player game object is missing Animator component.");
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Audio/Blaster") is null)
        {
            Debug.LogError(
                "ERROR: <Blaster> - Player/Audio/Blaster game object was not found in the game object " +
                "hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find("Player/Audio/Blaster").GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Blaster> - Player/Audio/Blaster game object is missing AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/UICanvas/HUD/BarsWrapper/BlasterBar/Slider") is null)
        {
            Debug.LogError(
                "ERROR: <Blaster> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/BlasterBar/Slider game " +
                "object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((BlasterHeatBar = GameObject.Find(
                    "Interface/MainCamera/UICanvas/HUD/BarsWrapper/BlasterBar/Slider"
                    ).GetComponent<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Blaster> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/BlasterBar/Slider game " +
                "object is missing BarManagement component."
            );
            Application.Quit(1);
        }

        BulletPrefab = Resources.Load("Prefabs/Objects/Bullet") as GameObject;
        if (BulletPrefab is null)
        {
            Debug.LogError("ERROR: <Blaster> - Prefabs/objects/Bullet resource was not loaded.");
            Application.Quit(1);
        }
        
        MaximumBlasterHeat = 10000;
        CurrentBlasterHeat = 0;
        BlasterHeatPerShot = 2000;
        BlasterCoolingStartTime = 0.2f;
        BlasterCoolingPower = 100;
        BlasterOverheatCoolingStartTime = 0.6f;
        BlasterOverheatCoolingPower = 60;
        BulletDamage = 2000;
        BulletSpeed = 20f;
        ShootCoroutine = null;
        CoolingCoroutine = null;
        OverheatCoroutine = null;
        BulletSound = "PlayerBlasterShotSound";
    }
    
    private void Start()
    {
        BlasterHeatBar.SetMaxValue(1f);
        BlasterHeatBar.SetValue(0f);
        BlasterHeatBar.SetGradient("Increasing");
    }
    
    public void Reload()
    {
        if (ShootCoroutine is not null)
        {
            Animator.SetBool("IsShooting", false);
            StopCoroutine(ShootCoroutine);
            ShootCoroutine = null;
        }
        
        if (CoolingCoroutine is not null)
        {
            StopCoroutine(CoolingCoroutine);
            CoolingCoroutine = null;
        }
        
        if (OverheatCoroutine is not null)
        {
            StopCoroutine(OverheatCoroutine);
            OverheatCoroutine = null;
        }

        CurrentBlasterHeat = 0;
        BlasterHeatBar.SetValue(0);
        BlasterHeatBar.SetGradient("Increasing");
    }
    
    public void Activate()
    {
        if (OverheatCoroutine is null)
        {
            if (ShootCoroutine != null)
            {
                StopCoroutine(ShootCoroutine);
            }
            ShootCoroutine = StartCoroutine(Shoot());
        }
        else
        {
            AudioManagement.PlayOneShot("AbilityNotAvailableSound");
        }
    }

    private IEnumerator Shoot()
    {
        Animator.SetBool("IsShooting", true);
        
        if (CoolingCoroutine is not null)
        {
            StopCoroutine(CoolingCoroutine);
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnBullet();
            CurrentBlasterHeat += BlasterHeatPerShot;
            BlasterHeatBar.SetValue((float) CurrentBlasterHeat / MaximumBlasterHeat);

            if (CurrentBlasterHeat >= MaximumBlasterHeat)
            {
                Animator.SetBool("IsShooting", false);
                
                CurrentBlasterHeat = MaximumBlasterHeat;
                OverheatCoroutine = StartCoroutine(Overheat());
                AudioManagement.PlayOneShot("PlayerBlasterOverheatSound");
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
        
        Animator.SetBool("IsShooting", false);
        
        CoolingCoroutine = StartCoroutine(Cooling());
        ShootCoroutine = null;
    }

    private IEnumerator Cooling()
    {
        yield return new WaitForSeconds(BlasterCoolingStartTime);

        while (CurrentBlasterHeat > BlasterCoolingPower)
        {
            yield return new WaitForSeconds(0.01f);
            CurrentBlasterHeat -= BlasterCoolingPower;
            BlasterHeatBar.SetValue((float) CurrentBlasterHeat / MaximumBlasterHeat);
        }
        
        CurrentBlasterHeat = 0;
        BlasterHeatBar.SetValue((float) CurrentBlasterHeat / MaximumBlasterHeat);
        CoolingCoroutine = null;
    }

    private IEnumerator Overheat()
    {
        AudioManagement.PlayOneShot("PlayerBlasterOverheatSound");
        BlasterHeatBar.SetGradient("Recharging");
        BlasterHeatBar.SetValue(1f);

        yield return new WaitForSeconds(BlasterOverheatCoolingStartTime);

        while (CurrentBlasterHeat > BlasterOverheatCoolingPower)
        {
            yield return new WaitForSeconds(0.01f);
            CurrentBlasterHeat -= BlasterOverheatCoolingPower;
            BlasterHeatBar.SetValue((float) CurrentBlasterHeat / MaximumBlasterHeat);
        }

        AudioManagement.PlayOneShot("PlayerBlasterRechargedSound");
        CurrentBlasterHeat = 0;
        BlasterHeatBar.SetGradient("Increasing");
        BlasterHeatBar.SetValue(0f);
        OverheatCoroutine = null;
    }

 
    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(
            BulletPrefab,
            this.gameObject.transform.position,
            this.gameObject.transform.rotation
        );
        bullet.GetComponent<Bullet>().Initialize(this.gameObject, BulletDamage, BulletSpeed);
        AudioManagement.PlayOneShot(BulletSound);
    }
}
