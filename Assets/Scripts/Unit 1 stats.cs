using UnityEngine;

public class Unit1stats : UnitBaseStats
{
    protected override void Start()
    {
        // Customize stats for Unit 2
        range = 30;
        damage = 10;
        attackSpeed = 0.8f;
        maxHealth = 100;
        unitValue = 30;

        // Call the base class's Start method
        base.Start();
    }
}
