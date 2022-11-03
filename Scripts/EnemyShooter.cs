using System.Collections;
using UnityEngine;

public abstract class EnemyShooter : Enemy
{
    protected Transform Firepoint { get; set; }
    private GameObject BulletPrefab { get; set; }
    protected Coroutine ShootCoroutine { get; set; }
    protected Coroutine ShootStartDelayCoroutine { get; set; }
    protected float ShootingRate { get; set; }
    protected int BulletsPerShot { get; set; }
    protected int BulletDamage { get; set; }
    protected float BulletSpeed { get; set; }
    protected string BulletSound { get; set; }
    protected float ViewRangeHorizontal { get; set; }
    protected float ViewRangeVertical { get; set; }

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

        if ((BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject) is null)
        {
            Debug.LogError("ERROR: <EnemyShooter> - Prefabs/Bullet resource was not loaded.");
            Application.Quit(1);
        }
    }

    protected new void Start()
    {
        base.Start();
        
        ShootCoroutine = null;
        ShootStartDelayCoroutine = null;
        ShootingRate = 1f;
        BulletsPerShot = 1;
        BulletDamage = 0;
        BulletSpeed = 20f;
        ViewRangeHorizontal = 10.5f;
        ViewRangeVertical = 1.8f;
        BulletSound = "EnemyBlasterShotSound";
    }

    protected new void Update()
    {
        base.Update();
        
        DetectPlayer();
    }

    protected void DetectPlayer()
    {
        // View range
        if (Player.transform.position.x > (this.transform.position.x - ViewRangeHorizontal) 
            && Player.transform.position.x < (this.transform.position.x + ViewRangeHorizontal)
            && Player.transform.position.y > (this.transform.position.y - ViewRangeVertical)
            && Player.transform.position.y < (this.transform.position.y + ViewRangeVertical))
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

    protected IEnumerator Shoot()
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
