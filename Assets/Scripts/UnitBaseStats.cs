using UnityEngine;

public class UnitBaseStats : MonoBehaviour
{
    public string team;           // Team name ("team 1" or "team 2")
    public float range = 50f;      // Attack range
    public int damage = 10;       // Attack damage
    public float attackSpeed = 0f; // Time between attacks
    public int maxHealth = 100;   // Maximum health
    public float unitValue = 50;    // Value when this unit dies

    private MoneyManager moneyMaker;

    protected int currentHealth;     // Current health of the unit

    protected virtual void Start()
    {
        // Initialize the unit's health
        currentHealth = maxHealth;
        moneyMaker = FindFirstObjectByType<MoneyManager>();
    Debug.Log($"{gameObject.name} initialized with max health: {maxHealth}");
    Debug.Log($"{gameObject.name} initialized with max range: {range}");
    Debug.Log($"{gameObject.name} initialized with max damage: {damage}");
    Debug.Log($"{gameObject.name} initialized with max attackSpeed: {attackSpeed}");
    Debug.Log($"{gameObject.name} initialized with max unitValue: {unitValue}");
    }

        Debug.Log($"{gameObject.name} initialized with max health: {maxHealth}");
        Debug.Log($"{gameObject.name} initialized with range: {range}");
        Debug.Log($"{gameObject.name} initialized with damage: {damage}");
        Debug.Log($"{gameObject.name} initialized with attack speed: {attackSpeed}");
        Debug.Log($"{gameObject.name} initialized with unit value: {unitValue}");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died! Unit value: {unitValue}");
        if (team.Equals("1"))
        {
            moneyMaker.SpendMoneyP2(-unitValue);
        }
        else
        {
            moneyMaker.SpendMoneyP1(-unitValue);
        }
        Destroy(gameObject); // Remove the unit from the scene

    }
}
