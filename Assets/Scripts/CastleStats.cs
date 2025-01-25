using UnityEngine;

public class CastleStats : MonoBehaviour
{
    public string team = "1";           // Team name ("team 1" or "team 2")
    //public float range = 3f;      // Attack range
    //public int damage = 10;       // Attack damage
    //public float attackSpeed = 0f; // Time between attacks
    public int maxHealth = 5000;   // Maximum health
    //public int unitValue = 50;    // Value when this unit dies

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    Debug.Log($"{gameObject.name} initialized with max health: {maxHealth}");
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
        Destroy(gameObject); // Remove the unit from the scene
    }
}

