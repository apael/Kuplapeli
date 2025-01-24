using UnityEngine;

public class Unit1stats : MonoBehaviour
{
    // Health
    public float maxHealth = 100f;
    private float currentHealth;

    // Damage
    public float damage = 10f;

    // Range
    public float range = 5f;

    // speed
    public float speed = 10f;
    private Vector2 movement;
    
    // value
    public float value = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize current health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Get input from arrow keys or WASD
        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Move the object
        GetComponent<Rigidbody2D>().linearVelocity = movement * speed;
    }

    // Method to apply damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Clamp health to avoid negative values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to simulate an attack (for demonstration purposes)
    public void Attack(GameObject target)
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance <= range)
        {
            Debug.Log($"{gameObject.name} attacked {target.name} for {damage} damage!");
            var targetStats = target.GetComponent<Unit1stats>();
            if (targetStats != null)
            {
                targetStats.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log($"Target is out of range! (Range: {range}, Distance: {distance})");
        }
    }

    // Handle death
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        // Optionally destroy the object
        Destroy(gameObject);
    }
}
