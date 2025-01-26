using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AttackMechanics : MonoBehaviour
{
    private UnitBaseStats BaseStats;
    private float nextAttackTime = 0f;
    private Transform targetEnemy;

    private SpriteSheetAnimator move;
    public Sprite projectileSprite;
    public Transform firePoint;
    public Canvas canvas;
    public GameObject projectilePrefab;
    void Start()
    {
        BaseStats = GetComponent<UnitBaseStats>();
        move = GetComponent<SpriteSheetAnimator>();

    }

    void Update()
    {
        if (targetEnemy != null)
        {
            float distance = Vector2.Distance(transform.position, targetEnemy.position);
            if (distance <= (BaseStats.range) && Time.time >= nextAttackTime)
            {
                ShootProjectile(BaseStats.damage, BaseStats.damageType);
                nextAttackTime = Time.time + BaseStats.attackSpeed;
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
            projectile.transform.SetParent(canvas.transform, false);
            StartCoroutine(MoveProjectile(projectile, targetEnemy));
        }
    }

    IEnumerator MoveProjectile(GameObject projectile, Transform targetEnemy)
    {

        if (projectile == null || targetEnemy == null || BaseStats == null)
        {
            Destroy(projectile);
            yield break;
        }

        Vector3 targetPosition = targetEnemy.position;

        while (projectile != null && targetEnemy != null)
        {
            // If the target has been destroyed or nullified during movement
            if (targetEnemy == null)
            {
                Destroy(projectile);
                yield break;
            }

            // Move the projectile towards the target position
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, 300f * Time.deltaTime);

            // If the projectile is close enough to the target, destroy it
            if (Vector3.Distance(projectile.transform.position, targetPosition) < 3f)
            {
                Destroy(projectile);
                break;
            }
            yield return null;
        }
        Destroy(projectile);
        if (targetEnemy != null)
        {
            UnitBaseStats enemyStats = targetEnemy.GetComponent<UnitBaseStats>();
            Attack(enemyStats);
        }
        else
        {
            Destroy(projectile);
        }
    }



    private void Attack(UnitBaseStats enemyStats)
    {
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(BaseStats.damage, BaseStats.damageType);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitBaseStats collisionStats = collision.GetComponent<UnitBaseStats>();
        if (collisionStats)
            if (!collisionStats.team.Equals(BaseStats.team) && !targetEnemy)
            {
                targetEnemy = collision.transform;
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
        float closestDistance = float.MaxValue;

        foreach (Collider2D enemy in enemiesInRange)
        {
            UnitBaseStats enemyStats = enemy.GetComponent<UnitBaseStats>();

            if (enemyStats != null && !enemyStats.team.Equals(BaseStats.team))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemyStats.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyStats.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy;
            move.targetEnemy = closestEnemy;
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
