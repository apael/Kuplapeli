using UnityEngine;

public class Unit2stats : UnitBaseStats
{
    void Start()
    {
        // Set unique values for Unit 1
        range = 2.5f;
        damage = 15;
        attackSpeed = 0.8f;
        maxHealth = 120;
        unitValue = 60;
    }
}