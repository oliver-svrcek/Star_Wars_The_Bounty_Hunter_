
public class EnemyRodian : EnemyShooter
{
    protected new void Start()
    {
        base.Start();

        CanHeal = false;
        MaximumHealth = 10000;
        CurrentHealth = MaximumHealth;
        ShootingRate = 1.1f;
        BulletsPerShot = 1;
        BulletDamage = 3300;
        BulletSpeed = 18f;
        DeathSound = "RodianDeathSound";
    }
}
