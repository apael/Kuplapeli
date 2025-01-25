using UnityEngine;

public class AttackMechanics : MonoBehaviour
{
    private UnitBaseStats BaseStats;         // Reference to the unit's stats
    private float nextAttackTime = 0f;      // Time tracker for attack cooldown
    private Transform targetEnemy;          // Current target within range

    private SpriteSheetAnimator move;

    void Start()
    {
        // Retrieve the UnitStats component attached to this GameObject
        BaseStats = GetComponent<UnitBaseStats>();
        move = GetComponent<SpriteSheetAnimator>();


        if (BaseStats == null)
        {
            Debug.LogError("UnitStats component is missing on " + gameObject.name);
            return;
        }
    }

    void Update()
    {
        // If there's a target and it's within range, attack
        if (targetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, targetEnemy.position);
            if (distance <= (BaseStats.range) && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + BaseStats.attackSpeed; // Attack cooldown
            }

        }
    }

    private void Attack()
    {
        // Access the enemy's UnitStats and apply damage
        UnitBaseStats enemyStats = targetEnemy.GetComponent<UnitBaseStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(BaseStats.damage);
            Debug.Log($"{gameObject?.name} attacked {targetEnemy?.name} for {BaseStats?.damage} damage!");

        }
        else
        {
            Debug.LogWarning("Enemy does not have UnitBaseStats component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitBaseStats collisionStats = collision.GetComponent<UnitBaseStats>();
        // Check if the object entering range is from the opposite team (not the same team)
        if (!collisionStats.team.Equals(BaseStats.team))
        {
            targetEnemy = collision.transform; // Set it as the target
            float distance = Vector2.Distance(transform.position, targetEnemy.position);

            if (distance <= (BaseStats.range))
            {
                move.targetEnemy = collision.transform;
                move.StopMovement();
            }
            Debug.Log($"{gameObject.name} detected {collision.name} as enemy.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        move.ContinueMovement();
        LookForNewEnemiesInRange();
    }



    private void LookForNewEnemiesInRange()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, BaseStats.range);
        foreach (Collider2D enemy in enemiesInRange)
        {
            // Check if the enemy has a UnitBaseStats component
            UnitBaseStats enemyStats = enemy.GetComponent<UnitBaseStats>();

            // Ensure enemyStats is not null and the enemy is on a different team
            if (enemyStats != null && !enemyStats.team.Equals(BaseStats.team))
            {
                // If a new enemy is found, set it as the target
                targetEnemy = enemyStats.transform;
                move.targetEnemy = enemyStats.transform;
                Debug.Log($"{gameObject.name} found new enemy: {enemyStats.gameObject.name}");
                break; // Optionally, break if you just need the first enemy
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the editor
        if (BaseStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, BaseStats.range);
        }
    }
}
