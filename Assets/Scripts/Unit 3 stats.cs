using UnityEngine;

public class Unit3stats : UnitBaseStats
{
    void Start()
    {
        // Set unique values for Unit 1
        range = 10;
        damage = 3;
        attackSpeed = 0.8f;
        maxHealth = 600;
        unitValue = 40;
        Debug.Log($"{gameObject.name} initialized with max health: {maxHealth}");
    Debug.Log($"{gameObject.name} initialized with max range: {range}");
    Debug.Log($"{gameObject.name} initialized with max damage: {damage}");
    Debug.Log($"{gameObject.name} initialized with max attackSpeed: {attackSpeed}");
    Debug.Log($"{gameObject.name} initialized with max unitValue: {unitValue}");
    }
}