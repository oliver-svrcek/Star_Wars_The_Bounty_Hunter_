using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyFlyer
{
    protected new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = false;
            MaximumHealth = 1;
            CurrentHealth = MaximumHealth;
            Damage = Int32.MaxValue;
            DeathSound = "MosquitoDeathSound";
        }
    }
}
