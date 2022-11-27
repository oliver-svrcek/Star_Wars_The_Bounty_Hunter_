using System;

public class EnemyMosquito : EnemyFlyer
{
    private new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = false;
            MaximumHealth = 1;
            CurrentHealth = MaximumHealth;
            Damage = int.MaxValue;
            DeathSound = "MosquitoDeathSound";
        }
    }
}
