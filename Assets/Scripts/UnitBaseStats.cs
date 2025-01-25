using UnityEngine;

public class UnitBaseStats : MonoBehaviour
{
    public string team = "Team 1";    // Default team
    public float range = 3f;         // Default attack range
    public int damage = 10;          // Default attack damage
    public float attackSpeed = 1f;   // Default time between attacks
    public int maxHealth = 100;      // Default maximum health
    public int unitValue = 50;       // Default value when this unit dies

    protected int currentHealth;     // Current health of the unit

    protected virtual void Start()
    {
        // Initialize the unit's health
        currentHealth = maxHealth;

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
        Destroy(gameObject); // Remove the unit from the scene
    }
}
