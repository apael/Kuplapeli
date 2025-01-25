using UnityEngine;

public class Unit2stats : UnitBaseStats
{
    protected override void Start()
    {
        // Customize stats for Unit 2
        range = 30;
        damage = 4;
        attackSpeed = 0.8f;
        maxHealth = 150;
        unitValue = 30;

        // Call the base class's Start method
        base.Start();
    }
}
