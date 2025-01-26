using UnityEngine;

public class UnitBaseStats : MonoBehaviour
{
    public string team;           // Team name ("team 1" or "team 2")
    public float range = 50f;      // Attack range
    public int damage = 10;       // Attack damage
    public float attackSpeed = 0f; // Time between attacks
    public int maxHealth = 100;   // Maximum health
    public float unitValue;  // Value when this unit dies
    public HpBar hpBar; // Reference to the health bar object
    public float speed;
    public enum ArmorType { Cloth, Heavy, Light }
    public enum DamageType { Magic, Normal, Pierce }
    public enum UnitClass { Mage, Archer, Guardian, Thief }

    public ArmorType armorType;
    public DamageType damageType;

    private MoneyManager moneyMaker;
    private SpriteCreator spriteCreator;

    public int currentHealth;     // Current health of the unit
    public UnitClass selectedClass;

    // Initialize unit with selected class
    public void SetUnitClass(UnitClass newClass)
    {
        selectedClass = newClass;

        switch (selectedClass)
        {
            case UnitClass.Mage:
                damage = 25;
                attackSpeed = 1.5f;
                maxHealth = 180;
                range = 150;
                armorType = ArmorType.Cloth;
                damageType = DamageType.Magic;
                unitValue = 10f;
                speed = 30f;

                break;
            case UnitClass.Archer:
                damage = 20;
                attackSpeed = 1.2f;
                maxHealth = 100;
                range = 200f;
                armorType = ArmorType.Light;
                damageType = DamageType.Pierce;
                unitValue = 10f;
                speed = 40f;

                break;
            case UnitClass.Guardian:
                damage = 7;
                attackSpeed = 0.8f;
                maxHealth = 300;
                range = 65;
                armorType = ArmorType.Heavy;
                damageType = DamageType.Normal;
                unitValue = 10f;
                speed = 60f;
                break;
            case UnitClass.Thief:
                damage = 18;
                attackSpeed = 1.8f;
                maxHealth = 130;
                range = 65;
                armorType = ArmorType.Light;
                damageType = DamageType.Pierce;
                unitValue = 10f;
                speed = 50f;

                break;
        }

        // Set current health to max health
        currentHealth = maxHealth;
    }
    protected virtual void Start()
    {

        // Initialize the unit's health
        moneyMaker = FindFirstObjectByType<MoneyManager>();
        spriteCreator =  FindFirstObjectByType<SpriteCreator>();
        SetUnitClass(selectedClass);
        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (team.Equals("1"))
        {
            spriteCreator.modifyCountP1(-1);
        }
        else
        {
            spriteCreator.modifyCountP2(-1);
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
                else if (defender.armorType == ArmorType.Cloth)
                    attackerDamage *= 0.75f; // Magic < Normal
                break;

            case DamageType.Pierce:
                if (defender.armorType == ArmorType.Light)
                    attackerDamage *= 1.5f; // Pierce > Light
                else if (defender.armorType == ArmorType.Heavy)
                    attackerDamage *= 0.5f; // Pierce < Heavy
                else if (defender.armorType == ArmorType.Cloth)
                    attackerDamage *= 0.75f; // Pierce < Cloth
                break;

            case DamageType.Normal:
                if (defender.armorType == ArmorType.Cloth)
                    attackerDamage *= 1.5f; // Normal > Cloth
                else if (defender.armorType == ArmorType.Light)
                    attackerDamage *= 0.5f; // Normal < Light
                else if (defender.armorType == ArmorType.Heavy)
                    attackerDamage *= 0.75f; // Normal < Heavy
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
            maxHealth = 180;
            range = 100f;
            armorType = ArmorType.Cloth;
            damageType = DamageType.Magic;
            unitValue = 10f;
            speed = 30f;
        }
    }

    // Subclass for Archer
    public class Archer : UnitBaseStats
    {
        public Archer()
        {
            damage = 15;
            attackSpeed = 1.2f;
            maxHealth = 100;
            range = 150f;
            armorType = ArmorType.Light;
            damageType = DamageType.Pierce;
            unitValue = 10f;
            speed = 40f;

        }
    }

    // Subclass for Guardian
    public class Guardian : UnitBaseStats
    {
        public Guardian()
        {
            damage = 5;
            attackSpeed = 0.8f;
            maxHealth = 300;
            range = 40f;
            armorType = ArmorType.Heavy;
            damageType = DamageType.Normal;
            unitValue = 10f;
            speed = 60f;
        }
    }

    // Subclass for Thief
    public class Thief : UnitBaseStats
    {
        public Thief()
        {
            damage = 18;
            attackSpeed = 1.8f;
            maxHealth = 130;
            range = 50f;
            armorType = ArmorType.Light;
            damageType = DamageType.Pierce;
            unitValue = 10f;
            speed = 50f;
        }
    }
}

