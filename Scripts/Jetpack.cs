using System;
using System.Collections;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    private Animator Animator { get; set; } = null;
    private CharacterMovementController CharacterMovementController { get; set; } = null;
    private Rigidbody2D Rigidbody2D { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private BarManagement JetpackFuelBar { get; set; } = null;
    private bool Fly { get; set; } = false;
    public Coroutine BurnFuelCoroutine { get; private set; } = null;
    public Coroutine RechargeFuelCoroutine { get; private set; } = null;
    private int MaximumJetpackFuel { get; set; } = 10000;
    private int CurrentJetpackFuel { get; set; } = 0;
    public int JetpackFuelConsumptionInitialPoints { get; set; } = 800;
    public int JetpackFuelConsumptionPoints { get; set; } = 70;
    public float JetpackFuelConsumptionRate { get; set; } = 0.01f;
    public int JetpackFuelRechargePoints { get; set; } = 30;
    public float JetpackFuelRechargeStartTime { get; set; } = 1.2f;
    public float JetpackFuelRechargeRate { get; set; } = 0.01f;

    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
		
        if ((Animator = GameObject.Find("Player").GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player game object is missing Animator component."
                );
            Application.Quit(1);
        }

        if ((CharacterMovementController = GameObject.Find(
                "Player"
            ).GetComponent<CharacterMovementController>()) is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player game object is missing CharacterMovementController component."
                );
            Application.Quit(1);
        }
        
        if ((Rigidbody2D = GameObject.Find("Player").GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player game object is missing Rigidbody2D component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Audio/Jetpack") is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player/Audio/Jetpack game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Player/Audio/Jetpack"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Player/Audio/Jetpack game object is missing AudioManagement component."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Interface/MainCamera/UICanvas/HUD/BarsWrapper/JetpackBar/Slider") is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/JetpackBar/Slider game " +
                "object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((JetpackFuelBar = GameObject.Find(
                "Interface/MainCamera/UICanvas/HUD/BarsWrapper/JetpackBar/Slider"
                ).GetComponent<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Jetpack> - Interface/MainCamera/UICanvas/HUD/BarsWrapper/JetpackBar/" +
                "Slider game object is missing BarManagement component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        CurrentJetpackFuel = MaximumJetpackFuel;
        AudioManagement.Play("FlameSoundLong", true);
        AudioManagement.SetPause(true);
        JetpackFuelBar.SetMaxValue(1f);
        JetpackFuelBar.SetValue(1f);
        JetpackFuelBar.SetGradient("Decreasing");
    }

    public void Reload()
    {
        if (BurnFuelCoroutine is not null)
        {
            Animator.SetBool("IsUsingJetpack", false);
            StopCoroutine(BurnFuelCoroutine);
            BurnFuelCoroutine = null;
        }
        
        if (RechargeFuelCoroutine is not null)
        {
            StopCoroutine(RechargeFuelCoroutine);
            RechargeFuelCoroutine = null;
        }

        CurrentJetpackFuel = MaximumJetpackFuel;
        
        JetpackFuelBar.SetMaxValue(1f);
        JetpackFuelBar.SetValue(1f);
        JetpackFuelBar.SetGradient("Decreasing");
    }
    
    public void Activate()
    {
        Fly = true;
    }

    public void Deactivate()
    {
        Fly = false;
    }
    
    private void FixedUpdate()
    {
        if (Fly && CurrentJetpackFuel > 0)
        {
            if (RechargeFuelCoroutine is not null)
            {
                StopCoroutine(RechargeFuelCoroutine);
                RechargeFuelCoroutine = null;
            }
            if (BurnFuelCoroutine is null)
            {
                BurnFuelCoroutine = StartCoroutine(BurnFuel());
            }
            
            FlyJetpack();
        }
        else
        {
            Animator.SetBool("IsUsingJetpack", false);
            if (!CharacterMovementController.IsGrounded && Rigidbody2D.velocity.y < 0.001f)
            {
                Animator.SetBool("IsFalling", true);
            }

            if (AudioManagement.IsPlaying())
            {
                AudioManagement.SetPause(true);
            }
        }
        
        if (!Fly)
        {
            if (BurnFuelCoroutine is not null)
            {
                Animator.SetBool("IsUsingJetpack", false);
                if (!CharacterMovementController.IsGrounded && Rigidbody2D.velocity.y < 0.001f)
                {
                    Animator.SetBool("IsFalling", true);
                }
                
                StopCoroutine(BurnFuelCoroutine);
                BurnFuelCoroutine = null;
            }
            
            if (CurrentJetpackFuel < MaximumJetpackFuel)
            {
                if (RechargeFuelCoroutine is null)
                {
                    RechargeFuelCoroutine = StartCoroutine(RechargeFuel());
                }
            }
        }
    }

    private void FlyJetpack()
    {
        if (!AudioManagement.IsPlaying())
        {
            AudioManagement.SetPause(false);
        }
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
        Rigidbody2D.AddForce(new Vector2(0f, 500f));
    }

    private IEnumerator BurnFuel()
    {
        Animator.SetBool("IsUsingJetpack", true);
        
        CurrentJetpackFuel -= JetpackFuelConsumptionInitialPoints;
        JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        
        while (CurrentJetpackFuel > 0)
        {
            yield return new WaitForSeconds(JetpackFuelConsumptionRate);
            CurrentJetpackFuel -= JetpackFuelConsumptionPoints;
            JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        }
        
        Animator.SetBool("IsUsingJetpack", false);
        if (!CharacterMovementController.IsGrounded)
        {
            Animator.SetBool("IsFalling", true);
        }

        CurrentJetpackFuel = 0;
        JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        BurnFuelCoroutine = null;
    }
    
    private IEnumerator RechargeFuel()
    {
        yield return new WaitForSeconds(JetpackFuelRechargeStartTime);
        JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        
        while (CurrentJetpackFuel < MaximumJetpackFuel)
        {
            yield return new WaitForSeconds(JetpackFuelRechargeRate);
            CurrentJetpackFuel += JetpackFuelRechargePoints;
            JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        }

        CurrentJetpackFuel = MaximumJetpackFuel;
        JetpackFuelBar.SetValue(((float) CurrentJetpackFuel / MaximumJetpackFuel));
        RechargeFuelCoroutine = null;
    }
}
