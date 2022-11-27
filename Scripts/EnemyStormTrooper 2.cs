public class EnemyStormTrooper : EnemyShooter
{
    protected new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = false;
            MaximumHealth = 30000;
            CurrentHealth = MaximumHealth;
            ShootingRate = 0.3f;
            BulletsPerShot = 1;
            BulletDamage = 2000;
            BulletSpeed = 22f;
            DeathSound = "StormTrooperDeathSound";
        }
    }
}