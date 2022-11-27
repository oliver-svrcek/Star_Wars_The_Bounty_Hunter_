public class EnemySpider : EnemyWalker
{
    protected new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = false;
            MaximumHealth = 20000;
            CurrentHealth = MaximumHealth;
            Speed = 35f;
            Damage = 4900;
            DeathSound = "SpiderDeathSound";
        }
    }
}
