using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public class BabyYoda : MonoBehaviour
{
    private AIPath AIPath { get; set; } = null;
    private AIDestinationSetter AIDestinationSetter { get; set; } = null;
    private Player Player { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private AudioManagement MusicAudioManagement { get; set; } = null;
    private Animator Animator { get; set; } = null;
    private CapsuleCollider2D BodyCollider { get; set; } = null;
    private Transform Firepoint { get; set; } = null;
    private GameObject BulletPrefab { get; set; } = null;
    private Coroutine ShootCoroutine { get; set; } = null;
    private float ShootingRate { get; set; } = 0.05f;
    private bool Discovered { get; set; } = false;
    private bool Activated { get; set; } = false;
    private bool IsLookingRight { get; set; } = true;
    private string BulletSound { get; set; } = "PlayerBlasterShotSound";
    [field: SerializeField] private int BulletDamage { get; set; } = 240000;
    [field: SerializeField] private float BulletSpeed { get; set; } = 20f;
    
    private void Awake()
    {
        if ((AIPath = this.gameObject.GetComponent<AIPath>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AIPath component."
            );
            Application.Quit(1);
        }
        
        if ((AIDestinationSetter = this.gameObject.GetComponent<AIDestinationSetter>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AIDestinationSetter component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError("ERROR: <BabyYoda> - Player game object was not found in game object hierarchy.");
            Application.Quit(1);
        }
        if ((Player = GameObject.Find("Player").GetComponent<Player>()) is null)
        {
            Debug.LogError("ERROR: <BabyYoda> - Player game object is missing Player component.");
            Application.Quit(1);
        }
 
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Music") is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - Interface/MainCamera/Audio/Music game object was not found in game" +
                " object hierarchy."
            );
            Application.Quit(1);
        }
        if ((MusicAudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Music"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - Interface/MainCamera/Audio/Music game object is missing " +
                "FadeManagement component."
            );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
            );
            Application.Quit(1);
        }
        
        if ((BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + " game object is missing " +
                "CapsuleCollider2D component."
            );
            Application.Quit(1);
        }
        
        if ((Firepoint = this.gameObject.transform.Find("Firepoint")) is null)
        {
            Debug.LogError(
                "ERROR: <BabyYoda> - " + this.gameObject.transform.name + "/Firepoint game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }

        if ((BulletPrefab = Resources.Load("Prefabs/Objects/Bullet") as GameObject) is null)
        {
            Debug.LogError("ERROR: <BabyYoda> - Prefabs/Objects/Bullet resource was not loaded.");
            Application.Quit(1);
        }
    }

    private void Start()
    {
        AIDestinationSetter.target = Player.transform;
        AIPath.canMove = false;
        AIPath.enabled = false;
    }
    
    private void Update()
    {
        LookAtPlayer();
        
        float horizontalDistance = Player.transform.position.x - this.transform.position.x;
        float verticalDistance = Player.transform.position.y - this.transform.position.y;

        if (Math.Abs(horizontalDistance) < 8 &&
            Math.Abs(verticalDistance) < 3 &&
            !Discovered)
        {
            MusicAudioManagement.Stop();
            MusicAudioManagement.PlaySequence(new [] {"BabyYodaDiscoveryMusic", "LevelMusic"} ,true);
            Discovered = true;
        }
        
        if (Math.Abs(horizontalDistance) < 3 &&
            Math.Abs(verticalDistance) < 3 &&
            !Activated && Input.GetKeyDown(KeyCode.E))
        {
            AudioManagement.PlayOneShot("CoinSpawnSound");
            AIPath.enabled = true;
            AIPath.canMove = true;
            Activated = true;
        }
        
        if (Input.GetKeyDown(KeyCode.B) && Activated && ShootCoroutine is null)
        {
            Animator.SetBool("IsShooting", true);
            ShootCoroutine = StartCoroutine(Shoot());
        }
        else if (Input.GetKeyUp(KeyCode.B) && Activated && ShootCoroutine is not null)
        {
            Animator.SetBool("IsShooting", false);
            StopCoroutine(ShootCoroutine);
            ShootCoroutine = null;
        }
    }
    
    private void LookAtPlayer()
    {
        if ((Player.transform.position.x > (this.transform.position.x + BodyCollider.size.x) && !IsLookingRight)
            || (Player.transform.position.x < (this.transform.position.x - BodyCollider.size.x) && IsLookingRight))
        {
            transform.Rotate(0f, 180f, 0f);
            IsLookingRight = !IsLookingRight;
        }
    }
    
    private IEnumerator Shoot()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(ShootingRate);
        }
    }
    

    protected void SpawnBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, Firepoint.position, Firepoint.rotation);
        bullet.GetComponent<Bullet>().Initialize(this.gameObject, BulletDamage, BulletSpeed);
        
        AudioManagement.PlayOneShot(BulletSound);
    }
}
