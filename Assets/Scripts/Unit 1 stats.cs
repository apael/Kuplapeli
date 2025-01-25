using UnityEngine;

public class Unit1Stats : UnitBaseStats
{
    void Start()
    {
        // Set unique values for Unit 1
        range = 2;
        damage = 1;
        attackSpeed = 0.8f;
        maxHealth = 120;
        unitValue = 60;
    }
}

