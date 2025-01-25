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

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        moneyMaker = FindFirstObjectByType<MoneyManager>();
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
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

