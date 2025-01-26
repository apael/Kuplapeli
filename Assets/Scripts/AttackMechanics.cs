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

        if (projectile == null || targetEnemy == null || BaseStats == null)
        {
            Destroy(projectile);
            yield break; // Exit early if the projectile or target is null
        }

        Vector3 targetPosition = targetEnemy.position;

        while (projectile != null && targetEnemy != null)
        {
            // If the target has been destroyed or nullified during movement
            if (targetEnemy == null)
            {
                Destroy(projectile);
                yield break; // Exit the coroutine early
            }

            // Move the projectile towards the target position
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, 300f * Time.deltaTime);

            // If the projectile is close enough to the target, destroy it
            if (Vector3.Distance(projectile.transform.position, targetPosition) < 3f)
            {
                Destroy(projectile);
                break; // Exit the loop after destroying the projectile
            }

            // Yield until the next frame
            yield return null;
        }
        Destroy(projectile);
        if (targetEnemy != null)
        {
            Attack(); // Perform the attack logic
        }
        else
        {

            Destroy(projectile);
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
            if (!collisionStats.team.Equals(BaseStats.team) && !targetEnemy)
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
