using UnityEngine;

public class AttackMechanics : MonoBehaviour
{
    private UnitBaseStats BaseStats;         // Reference to the unit's stats
    private float nextAttackTime = 0f;      // Time tracker for attack cooldown
    public Transform targetEnemy;          // Current target within range

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

        // Optional: Set the enemyTag dynamically based on team (if you want)
        // enemyTag = BaseStats.team == "team 1" ? "team 2" : "team 1";
    }

    void Update()
{
    // If there's a target and it's within range, attempt to attack
    if (targetEnemy != null)
    {

        // If there's a target and it's within range, attack
        if (targetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, targetEnemy.position);
            if (distance <= BaseStats.range && Time.time >= nextAttackTime)
            {
                move.StopMovement();
                Attack();
                nextAttackTime = Time.time + BaseStats.attackSpeed; // Attack cooldown
            }

        }
        else
        {
            move.ContinueMovement();

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
            Debug.Log($"{gameObject.name} detected {collision.name} as enemy.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UnitBaseStats collisionStats = collision.GetComponent<UnitBaseStats>();
        if (!collisionStats.team.Equals(BaseStats.team))
        {
            Debug.Log($"{gameObject.name} lost sight of {collision.name}. Clearing target.");
            targetEnemy = null;
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
