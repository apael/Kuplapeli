using UnityEngine;

public class Unit1stats : UnitBaseStats
{
    protected override void Start()
    {
        // Customize stats for Unit 2
        range = 1;
        damage = 10;
        attackSpeed = 0.8f;
        maxHealth = 200;
        unitValue = 30;

        // Call the base class's Start method
        base.Start();
    }
}
