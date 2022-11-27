using System.Collections;
using UnityEngine;
using Random=UnityEngine.Random;

public class AbalKarda : EnemyShooter
{
    [field: SerializeField]  private float BarrageChange { get; set; } = 0.2f;
    
    private new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = true;
            MaximumHealth = 100000;
            CurrentHealth = MaximumHealth;
            BulletDamage = 3700;
            BulletsPerShot = 1;
            BulletSpeed = 18f;
            BarrageChange = 0.2f;
            HealStartTime = 3f;
            HealPoints = 10;
            DeathSound = "AbalKardaDeathSound";
        }
        
        StartCoroutine(Shoot());
    }
    
    private new void Update()
    {
        LookAtPlayer();
    }
    
    private new IEnumerator Shoot()
    {
        while (true)
        {
            // Ultimate ability - Barrage.
            if (Random.value >= 1 - BarrageChange)
            {
                BulletsPerShot = 5;
            }
            else
            {
                BulletsPerShot = 1;
            }
            
            for (int i = 0; i < BulletsPerShot; i++)
            {
                SpawnBullet();
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(ShootingRate);
        }
    }
}
