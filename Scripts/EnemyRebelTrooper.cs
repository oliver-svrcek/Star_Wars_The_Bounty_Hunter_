    
public class EnemyRebelTrooper : EnemyShooter
{
    protected new void Start()
    {
        base.Start();
        
        CanHeal = false;
        MaximumHealth = 20000;
        CurrentHealth = MaximumHealth;
        ShootingRate = 0.8f;
        BulletsPerShot = 3;
        BulletDamage = 2000;
        BulletSpeed = 22f;
        DeathSound = "RebelTrooperDeathSound";
    }
}
