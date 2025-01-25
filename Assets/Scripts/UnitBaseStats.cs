using UnityEngine;

public class UnitBaseStats : MonoBehaviour
{
    public string team;           // Team name ("team 1" or "team 2")
    public float range = 50f;      // Attack range
    public int damage = 10;       // Attack damage
    public float attackSpeed = 0f; // Time between attacks
    public int maxHealth = 100;   // Maximum health
    public float unitValue = 10;  // Value when this unit dies
    public HpBar hpBar; // Reference to the health bar object
    public enum ArmorType { Cloth, Heavy, Light }
    public enum DamageType { Magic, Normal, Pierce }
    public enum UnitClass { Mage, Archer, Guardian, Thief }

    public ArmorType armorType;
    public DamageType damageType;

    private MoneyManager moneyMaker;

    public int currentHealth;     // Current health of the unit
    public UnitClass selectedClass;

    // Initialize unit with selected class
    public void SetUnitClass(UnitClass newClass)
    {
        selectedClass = newClass;

        switch (selectedClass)
        {
            case UnitClass.Mage:
                damage = 20;
                attackSpeed = 1.5f;
                maxHealth = 80;
                range = 100f;
                armorType = ArmorType.Cloth;
                damageType = DamageType.Magic;
                break;
            case UnitClass.Archer:
                damage = 15;
                attackSpeed = 1.2f;
                maxHealth = 90;
                range = 150f;
                armorType = ArmorType.Light;
                damageType = DamageType.Pierce;
                break;
            case UnitClass.Guardian:
                damage = 10;
                attackSpeed = 0.8f;
                maxHealth = 150;
                range = 40f;
                armorType = ArmorType.Heavy;
                damageType = DamageType.Normal;
                break;
            case UnitClass.Thief:
                damage = 18;
                attackSpeed = 1.8f;
                maxHealth = 70;
                range = 50f;
                armorType = ArmorType.Light;
                damageType = DamageType.Pierce;
                break;
        }

        // Set current health to max health
        currentHealth = maxHealth;
    }
    protected virtual void Start()
    {

        // Initialize the unit's health
        currentHealth = maxHealth;
        moneyMaker = FindFirstObjectByType<MoneyManager>();
        hpBar.setMaxHealth(maxHealth);

    }
    public void TakeDamage(int baseDamage, DamageType attackerDamageType)
    {
        // Modify damage based on damage and armor types
        float finalDamage = baseDamage;

        switch (attackerDamageType)
        {
            case DamageType.Magic:
                if (armorType == ArmorType.Heavy)
                    finalDamage *= 1.5f; // Magic > Heavy
                else if (armorType == ArmorType.Light)
                    finalDamage *= 0.5f; // Magic < Light
                else if (armorType == ArmorType.Cloth)
                    finalDamage *= 0.75f; // Magic < Normal
                break;

            case DamageType.Pierce:
                if (armorType == ArmorType.Light)
                    finalDamage *= 1.5f; // Pierce > Light
                else if (armorType == ArmorType.Heavy)
                    finalDamage *= 0.5f; // Pierce < Heavy
                else if (armorType == ArmorType.Cloth)
                    finalDamage *= 0.75f; // Pierce < Cloth
                break;

            case DamageType.Normal:
                if (armorType == ArmorType.Cloth)
                    finalDamage *= 1.5f; // Normal > Cloth
                else if (armorType == ArmorType.Light)
                    finalDamage *= 0.5f; // Normal < Light
                else if (armorType == ArmorType.Heavy)
                    finalDamage *= 0.75f; // Normal < Heavy
                break;
        }

        // Apply damage to health
        currentHealth -= Mathf.RoundToInt(finalDamage);
        hpBar.setHealth(currentHealth);

        Debug.Log($"{gameObject.name} took {Mathf.RoundToInt(finalDamage)} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died! Unit value: {unitValue}");
        if (team.Equals("1"))
        {
            moneyMaker.SpendMoneyP2(-unitValue);
        }
        else
        {
            moneyMaker.SpendMoneyP1(-unitValue);
        }
        Destroy(gameObject); // Remove the unit from the scene

    }

    public static float CalculateCombatOutcome(UnitBaseStats attacker, UnitBaseStats defender)
    {
        float attackerDamage = attacker.damage; // Simplified, use damage for now
        float defenderDamage = defender.damage;

        // Damage calculations based on armor and damage types (simplified)
        switch (attacker.damageType)
        {
            case DamageType.Magic:
                if (defender.armorType == ArmorType.Heavy)
                    attackerDamage *= 1.5f; // Magic > Heavy
                else if (defender.armorType == ArmorType.Light)
                    attackerDamage *= 0.5f; // Magic < Light
                break;

            case DamageType.Pierce:
                if (defender.armorType == ArmorType.Light)
                    attackerDamage *= 1.5f; // Pierce > Light
                else if (defender.armorType == ArmorType.Heavy)
                    attackerDamage *= 0.5f; // Pierce < Heavy
                break;

            case DamageType.Normal:
                if (defender.armorType == ArmorType.Cloth)
                    attackerDamage *= 1.5f; // Normal > Cloth
                else if (defender.armorType == ArmorType.Light)
                    attackerDamage *= 0.5f; // Normal < Light
                break;
        }

        // Simulate the fight outcome (simplified: who deals more damage)
        float winProbability = attackerDamage / (attackerDamage + defenderDamage);
        return winProbability;
    }

    public class Mage : UnitBaseStats
    {
        public Mage()
        {
            damage = 20;
            attackSpeed = 1.5f;
            maxHealth = 80;
            range = 100f;
            armorType = ArmorType.Cloth;
            damageType = DamageType.Magic;
        }
    }

    // Subclass for Archer
    public class Archer : UnitBaseStats
    {
        public Archer()
        {
            damage = 15;
            attackSpeed = 1.2f;
            maxHealth = 90;
            range = 150f;
            armorType = ArmorType.Light;
            damageType = DamageType.Pierce;
        }
    }

    // Subclass for Guardian
    public class Guardian : UnitBaseStats
    {
        public Guardian()
        {
            damage = 10;
            attackSpeed = 0.8f;
            maxHealth = 150;
            range = 40f;
            armorType = ArmorType.Heavy;
            damageType = DamageType.Normal;
        }
    }

    // Subclass for Thief
    public class Thief : UnitBaseStats
    {
        public Thief()
        {
            damage = 18;
            attackSpeed = 1.8f;
            maxHealth = 70;
            range = 50f;
            armorType = ArmorType.Light;
            damageType = DamageType.Pierce;
        }
    }
}

