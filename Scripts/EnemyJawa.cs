public class EnemyJawa: EnemyChaser
{
    protected new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = false;
            MaximumHealth = 30000;
            CurrentHealth = MaximumHealth;
            Speed = 25f;
            Damage = 7400;
            DeathSound = "JawaDeathSound";
        }
    }
}