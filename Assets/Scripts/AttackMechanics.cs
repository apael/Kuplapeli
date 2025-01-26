using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AttackMechanics : MonoBehaviour
{
    private UnitBaseStats BaseStats;         // Reference to the unit's stats
    private float nextAttackTime = 0f;      // Time tracker for attack cooldown
    private Transform targetEnemy;          // Current target within range

    private SpriteSheetAnimator move;
    public Sprite projectileSprite;  // The projectile prefab to instantiate
    public Transform firePoint;  // The point from where the projectile will be shot
    public Canvas canvas; // Reference to your Canvas
    public GameObject projectilePrefab;
    void Start()
    {
        BaseStats = GetComponent<UnitBaseStats>();
        move = GetComponent<SpriteSheetAnimator>();

    }

    void Update()
    {
        // If there's a target and it's within range, attack
        if (targetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, targetEnemy.position);
            if (distance <= (BaseStats.range) && Time.time >= nextAttackTime)
            {
                ShootProjectile(BaseStats.damage, BaseStats.damageType);
                // Attack();
                nextAttackTime = Time.time + BaseStats.attackSpeed; // Attack cooldown
            }

        }
        else
        {
            move.ContinueMovement();
        }
    }

    void ShootProjectile(int damage, DamageType type)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(firePoint.position);

            // Convert screen position to local position in the Canvas (UI space)
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, Camera.main, out localPosition);

            // Set the position of the sprite object in the UI (canvas)
            projectile.GetComponent<RectTransform>().localPosition = localPosition;

            // Optional: Set the parent to the Canvas
            projectile.transform.SetParent(canvas.transform, false);


            // Move the projectile across the screen
            StartCoroutine(MoveProjectile(projectile, targetEnemy));
        }
    }

    IEnumerator MoveProjectile(GameObject projectile, Transform targetEnemy)
    {
        Vector3 startPosition = projectile.transform.position;
        Vector3 targetPosition = targetEnemy.position;
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(targetEnemy.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), targetScreenPosition, null, out Vector2 targetLocalPosition);


        while (projectile != null && projectile.transform.position != targetEnemy.position)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, 300f * Time.deltaTime);
            if (Vector3.Distance(projectile.transform.position, targetPosition) < 3f)
            {
                Destroy(projectile);
                break;
            }
            yield return null;
        }
        if (targetEnemy != null)
        {
            Attack(); // Perform the attack logic
        }
    }



    private void Attack()
    {
        // Access the enemy's UnitStats and apply damage
        UnitBaseStats enemyStats = targetEnemy.GetComponent<UnitBaseStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(BaseStats.damage, BaseStats.damageType);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitBaseStats collisionStats = collision.GetComponent<UnitBaseStats>();
        // Check if the object entering range is from the opposite team (not the same team)
        if (collisionStats)
            if (!collisionStats.team.Equals(BaseStats.team))
            {
                targetEnemy = collision.transform; // Set it as the target
                float distance = Vector2.Distance(transform.position, targetEnemy.position);

                if (distance <= (BaseStats.range))
                {
                    move.targetEnemy = collision.transform;
                    move.StopMovement();
                }
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!targetEnemy)
        {
            move.ContinueMovement();
        }
        LookForNewEnemiesInRange();
    }



private void LookForNewEnemiesInRange()
{
    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, BaseStats.range);
    Transform closestEnemy = null;
    float closestDistance = float.MaxValue; // Start with a very large distance

    foreach (Collider2D enemy in enemiesInRange)
    {
        // Check if the enemy has a UnitBaseStats component
        UnitBaseStats enemyStats = enemy.GetComponent<UnitBaseStats>();

        // Ensure enemyStats is not null and the enemy is on a different team
        if (enemyStats != null && !enemyStats.team.Equals(BaseStats.team))
        {
            // Calculate the distance to the current enemy
            float distanceToEnemy = Vector2.Distance(transform.position, enemyStats.transform.position);

            // If this enemy is closer than the previous closest, update target
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemyStats.transform;
            }
        }
    }

    // If a closest enemy is found, set it as the target
    if (closestEnemy != null)
    {
        targetEnemy = closestEnemy;
        move.targetEnemy = closestEnemy; // Update the move target as well
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
