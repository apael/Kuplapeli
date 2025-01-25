using UnityEngine;

public class CastleStats : MonoBehaviour
{
    public int maxHealth = 100;   // Maximum health
    public int currentHealth;     // Current health of the unit
    public HpBar hpBar;
    public GameManager gameManager;  // Reference to the GameManager
    private void Start()
    {
        currentHealth = maxHealth;
        hpBar.setMaxHealth(maxHealth);
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.HideGameOverPanel();

    }

    public void setHp(int health)
    {
        currentHealth -= health;
        hpBar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            gameManager.GameOver();  // Pass the current instance of CastleStats
        }
    }
}
