using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;  // The damage the projectile will deal
    public DamageType damageType;  // The type of damage (Magic, Normal, Pierce)
    public string team;
    public Canvas canvas;



public class ProjectileController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Canvas overlayCanvas; // Reference to your Canvas
    public Camera mainCamera; // Reference to your main camera


}}
