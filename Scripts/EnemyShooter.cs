using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyShooter : Enemy
{
    private Transform Firepoint { get; set; } = null;
    private GameObject BulletPrefab { get; set; } = null;
    private Coroutine ShootCoroutine { get; set; } = null;
    private Coroutine ShootStartDelayCoroutine { get; set; } = null;
    [field: SerializeField] protected float ShootingRate { get; set; } = 1f;
    [field: SerializeField] protected int BulletsPerShot { get; set; } = 1;
    [field: SerializeField] protected int BulletDamage { get; set; } = 0;
    [field: SerializeField] protected float BulletSpeed { get; set; } = 20f;
    [field: SerializeField] private string BulletSound { get; set; } = "EnemyBlasterShotSound";
    [field: SerializeField] private float ViewRangeHorizontal { get; set; } = 11;
    [field: SerializeField] private float ViewRangeVertical { get; set; } = 1.8f;

    protected new void Awake()
    {
        base.Awake();

        if ((Firepoint = this.gameObject.transform.Find("Firepoint")) is null)
        {
            Debug.LogError(
                "ERROR: <EnemyShooter> - " + this.gameObject.transform.name + "/Firepoint game object was " +
                "not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((BulletPrefab = Resources.Load("Prefabs/Objects/Bullet") as GameObject) is null)
        {
            Debug.LogError("ERROR: <EnemyShooter> - Prefabs/Objects/Bullet resource was not loaded.");
            Application.Quit(1);
        }
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        float horizontalDistance = Player.transform.position.x - this.transform.position.x;
        float verticalDistance = Player.transform.position.y - this.transform.position.y;
        
        
        if (Math.Abs(horizontalDistance) < ViewRangeHorizontal &&
            Math.Abs(verticalDistance) < ViewRangeVertical)
        {
            if (ShootCoroutine is null && ShootStartDelayCoroutine is null)
            {
                ShootCoroutine = StartCoroutine(Shoot());
            }
        }
        else if (ShootCoroutine is not null)
        {
            StopCoroutine(ShootCoroutine);
            ShootCoroutine = null;

            if (ShootStartDelayCoroutine is null)
            {
                ShootStartDelayCoroutine = StartCoroutine(ShootStartDelay());
            }
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            for (int i = 0; i < BulletsPerShot; i++)
            {
                SpawnBullet();
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(ShootingRate);
        }
    }
    
    private IEnumerator ShootStartDelay()
    {
        yield return new WaitForSeconds(ShootingRate);
        ShootStartDelayCoroutine = null;
    }

    protected void SpawnBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, Firepoint.position, Firepoint.rotation);
        bullet.GetComponent<Bullet>().Initialize(this.gameObject, BulletDamage, BulletSpeed);
        
        AudioManagement.PlayOneShot(BulletSound);
    }
}
