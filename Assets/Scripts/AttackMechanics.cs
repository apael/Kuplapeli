using UnityEngine;

public class AttackMechanics : MonoBehaviour
{
    private UnitStats unitStats;         // Reference to the unit's stats
    private float nextAttackTime = 0f;   // Time tracker for attack cooldown
    private Transform targetEnemy;       // Current target within range
    private string enemyTag;             // Tag of the opposing team

    void Start()
    {
        // Retrieve the UnitStats component attached to this GameObject
        unitStats = GetComponent<UnitStats>();

        if (unitStats == null)
        {
            Debug.LogError("UnitStats component is missing on " + gameObject.name);
            return;
        }

        // Determine the enemy's tag based on this unit's team
        enemyTag = unitStats.team == "team 1" ? "team 2" : "team 1";
    }

    void Update()
    {
        // If there's a target and it's within range, attack
        if (targetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, targetEnemy.position);
            if (distance <= unitStats.range && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + unitStats.attackSpeed; // Attack cooldown
            }
        }
    }

    private void Attack()
    {
        // Access the enemy's UnitStats and apply damage
        UnitStats enemyStats = targetEnemy.GetComponent<UnitStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(unitStats.damage);

            // Debug message for damage dealt
            Debug.Log($"{gameObject.name} attacked {targetEnemy.name} for {unitStats.damage} damage!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering range is on the opposing team
        if (collision.CompareTag(enemyTag))
        {
            targetEnemy = collision.transform; // Set it as the target
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Clear the target if it leaves the range
        if (collision.CompareTag(enemyTag) && collision.transform == targetEnemy)
        {
            targetEnemy = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the editor
        if (unitStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, unitStats.range);
        }
    }
}
