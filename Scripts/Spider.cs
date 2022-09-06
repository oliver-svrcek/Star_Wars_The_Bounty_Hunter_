public class Spider : EnemyWalker
{
    protected new void Start()
    {
        base.Start();

        CanHeal = false;
        MaximumHealth = 20000;
        CurrentHealth = MaximumHealth;
        Speed = 8f;
        Damage = 4900;
        DeathSound = "SpiderDeathSound";
    }
}
