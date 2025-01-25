using UnityEngine;
using TMPro;  // Required for TextMeshPro components

public class MoneyManager : MonoBehaviour
{
    // Global variable for money (static so it's accessible across scripts)
    public static float p1Money = 100f;
    public static float p2Money = 100f;

    public TextMeshProUGUI moneyTextP1;
    public TextMeshProUGUI moneyTextP2;
    public float IncomeP1;
    public float IncomeP2;
    private float updateTimer = 0f;
    private float updateCooldown = 5f;

    // Start is called before the first frame update
    void Start()
    {
        p1Money = 100f;
        p2Money = 100f;
        UpdateMoneyDisplay(); // Display the initial money value
        IncomeP2 = 0;
        IncomeP1 = 0;
    }


    void Update()
    {
        updateTimer += Time.deltaTime; // Increment the timer
        if (updateTimer >= updateCooldown) // Check if enough time has passed
        {
            updateTimer = 0f; // Reset the timer
            SpendMoneyP2(-IncomeP2);
            SpendMoneyP1(-IncomeP1);
        }

    }

    // Method to reduce money when the button is clicked
    public bool SpendMoneyP1(float amount)
    {
        if (p1Money >= amount)
        {
            p1Money -= amount;  // Reduce the money by the specified amount
            UpdateMoneyDisplay();  // Update the displayed money
            IncomeP1 += 1;
            return true;

        }
        else
        {
            return false;
        }
    }

    public bool SpendMoneyP2(float amount)
    {
        if (p2Money >= amount)
        {
            p2Money -= amount;  // Reduce the money by the specified amount
            UpdateMoneyDisplay();  // Update the displayed money
            IncomeP2 += 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Method to handle the button click (no parameter)
    public void SpendMoneyOnButtonClickP1()
    {
        float amountToReduce = 10f;  // Specify the amount to reduce
        SpendMoneyP1(amountToReduce);  // Call SpendMoney with this amount
    }


    public void SpendMoneyOnButtonClickP2()
    {
        float amountToReduce = 10f;  // Specify the amount to reduce
        SpendMoneyP2(amountToReduce);  // Call SpendMoney with this amount
    }
    
    public void increaseIncomeP2(float amount)
    {
        IncomeP2 += amount;
    }

    // Update the UI TextMeshPro with the current money
    void UpdateMoneyDisplay()
    {

        moneyTextP1.text = "Money: " + p1Money.ToString("F2");  // Display money with 2 decimal places
        moneyTextP2.text = "Money: " + p2Money.ToString("F2");  // Display money with 2 decimal places
    }
}
