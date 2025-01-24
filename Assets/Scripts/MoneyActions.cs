using UnityEngine;
using TMPro;  // Required for TextMeshPro components

public class MoneyManager : MonoBehaviour
{
    // Global variable for money (static so it's accessible across scripts)
    public static float p1Money = 100f;
    public static float p2Money = 100f;

    // UI TextMeshPro to display money (using TextMeshProUGUI instead of Text)
    public TextMeshProUGUI moneyTextP1;
    public TextMeshProUGUI moneyTextP2;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyDisplay(); // Display the initial money value
    }

    // Method to reduce money when the button is clicked
    public void SpendMoneyP1(float amount)
    {
        if (p1Money >= amount)
        {
            p1Money -= amount;  // Reduce the money by the specified amount
            UpdateMoneyDisplay();  // Update the displayed money
        }
        else
        {
            Debug.Log("Not enough money!");  // If there's not enough money
        }
    }

        public void SpendMoneyP2(float amount)
    {
        if (p2Money >= amount)
        {
            p2Money -= amount;  // Reduce the money by the specified amount
            UpdateMoneyDisplay();  // Update the displayed money
        }
        else
        {
            Debug.Log("Not enough money!");  // If there's not enough money
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

    // Update the UI TextMeshPro with the current money
    void UpdateMoneyDisplay()
    {

            moneyTextP1.text = "Money: " + p1Money.ToString("F2");  // Display money with 2 decimal places
            moneyTextP2.text = "Money: " + p2Money.ToString("F2");  // Display money with 2 decimal places
    }
}
