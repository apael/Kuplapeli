using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpriteCreator : MonoBehaviour
{
    private Coroutine moveCoroutine;
    // The Canvas where the sprite will be placed (if it's a UI element)
    public Canvas canvas;

    public GameObject[] predefinedObjectsP1;
    public GameObject[] predefinedObjectsP2;

    public TextMeshProUGUI weightsTextP1;
    public TextMeshProUGUI weightsTextP2;

    public UnitBaseStats[] unitClasses;

    // Weights for each sprite (higher values mean higher chance of selection)

    //P1
    public float sprite1WeightP1 = 1f;
    public float sprite2WeightP1 = 2f;
    public float sprite3WeightP1 = 3f;
    public float sprite4WeightP1 = 4f;

    //P2
    public float sprite1WeightP2 = 1f;
    public float sprite2WeightP2 = 2f;
    public float sprite3WeightP2 = 3f;
    public float sprite4WeightP2 = 4f;

    private float updateCooldown = 0.3f; // Time in seconds between updates
    private float updateTimer = 0f;

    // Castles
    public GameObject CastleP1;
    public GameObject CastleP2;

    private MoneyManager money;

    public float winProbabilityThreshold = 0.5f; // Threshold for deciding re-roll

    public bool aiOn;
    public GameObject aiCastle;
    private bool giftGiven = false;

    public bool loadedToggleState;
    
    private void Start()
    {
        loadedToggleState = PlayerPrefs.GetInt("ToggleState", 0) == 1;
        Debug.Log("Loaded Toggle State: " + loadedToggleState);
        // Automatically start animation when the GameObject is created
        money = GetComponent<MoneyManager>();
        UpdateWeightsTextP2();
        UpdateWeightsTextP1();
        unitClasses = new UnitBaseStats[]
        {
            new UnitBaseStats.Mage(),
            new UnitBaseStats.Archer(),
            new UnitBaseStats.Guardian(),
            new UnitBaseStats.Thief()
        };
        purchaseP1Text.text = $"{(10f * Mathf.Pow(growthRate, unitCountP1)):F1}";
        purchaseP2Text.text = $"{(10f * Mathf.Pow(growthRate, unitCountP2)):F1}";
        aiOn = loadedToggleState;

    }

    void Update()
    {

        if (aiOn == true)
        {
            updateTimer += Time.deltaTime;

            if (updateTimer >= updateCooldown)
            {
                updateTimer = 0f;

                float side1WinProbability = CalculateSideWinProbability();

                if (side1WinProbability > winProbabilityThreshold)
                {
                    UpdateWeightsP2(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
                }
                else
                {
                    sendUnitP2();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                sendUnitP2();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                reRollP2();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            sendUnitP1();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            reRollP1();
        }
    }


    public int modifyCountP1(int amount)
    {
        unitCountP1 += amount;
        purchaseP1Text.text = $"{(10f * Mathf.Pow(growthRate, unitCountP1)):F1}";
        return unitCountP1;
    }
    public int modifyCountP2(int amount)
    {
        unitCountP2 += amount;
        purchaseP2Text.text = $"{(10f * Mathf.Pow(growthRate, unitCountP2)):F1}";
        return unitCountP2;
    }



    public void sendUnitP1()
    {
        bool hasMoney = money.SpendMoneyP1(10f * Mathf.Pow(growthRate, unitCountP1));
        if (hasMoney)
        {
            modifyCountP1(1);
            CreateAndMoveRandomSprite(CastleP1, CastleP2, sprite1WeightP1, sprite2WeightP1, sprite3WeightP1, sprite4WeightP1, predefinedObjectsP1, "1", "Team 1");
        }
    }


    public void sendUnitP2()
    {
        bool hasMoney = money.SpendMoneyP2(10f * Mathf.Pow(growthRate, unitCountP2));
        if (hasMoney)
        {
            modifyCountP2(1);
            CreateAndMoveRandomSprite(CastleP2, CastleP1, sprite1WeightP2, sprite2WeightP2, sprite3WeightP2, sprite4WeightP2, predefinedObjectsP2, "2", "Team 2");
        }
    }
    // This method will be called by the button OnClick to create a random sprite and move it
    public void CreateAndMoveRandomSprite(GameObject ownCastle, GameObject enemyCastle, float sp1, float sp2, float sp3, float sp4, GameObject[] predefinedObjects, string team, string tag)
    {
        float[] weights = { sp1, sp2, sp3, sp4 };

        // Choose a random index based on weights
        int randomIndex = GetRandomIndexByWeight(weights);

        GameObject spriteObject = Instantiate(predefinedObjects[randomIndex]);
        spriteObject.name = "RandomSprite_" + randomIndex;

        UnitBaseStats baseStats = spriteObject.GetComponent<UnitBaseStats>();
        CircleCollider2D collider = spriteObject.GetComponent<CircleCollider2D>();
        baseStats.team = team;
        spriteObject.tag = tag;
        collider.radius = baseStats.range;

        // Convert the castle position to screen space
        Vector3 castleWorldPosition = ownCastle.transform.position;

        // Optional: Add an offset to spawn the sprite above the castle
        castleWorldPosition.y += Random.Range(-100.0f, 100.0f); // You can adjust this value to control how high the sprite spawns

        // Convert the world position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(castleWorldPosition);

        // Convert screen position to local position in the Canvas (UI space)
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, Camera.main, out localPosition);

        // Set the position of the sprite object in the UI (canvas)
        spriteObject.GetComponent<RectTransform>().localPosition = localPosition;

        // Optional: Set the parent to the Canvas
        spriteObject.transform.SetParent(canvas.transform, false);

        SpriteSheetAnimator controller = spriteObject.GetComponent<SpriteSheetAnimator>();

        if (controller != null)
        {
            controller.StartMovingSprite(spriteObject, enemyCastle, baseStats.speed);
        }
    }



    private int GetRandomIndexByWeight(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float totalWeight = 0f;
        foreach (float weight in weights) totalWeight += Mathf.Max(0, weight);

        if (totalWeight <= 0f) return -1;

        float randomValue = Random.Range(0f, totalWeight);
        for (int i = 0; i < weights.Length; i++)
        {
            if ((randomValue -= weights[i]) < 0f) return i;
        }

        return weights.Length - 1;
    }


    // This method will update the weights when the second button is pressed
    public void UpdateWeightsP1(float sp1, float sp2, float sp3, float sp4)
    {
        sprite1WeightP1 = sp1;
        sprite2WeightP1 = sp2;
        sprite3WeightP1 = sp3;
        sprite4WeightP1 = sp4;
        UpdateWeightsTextP1();

    }

    public void UpdateWeightsP2(float sp1, float sp2, float sp3, float sp4)
    {
        sprite1WeightP2 = sp1;
        sprite2WeightP2 = sp2;
        sprite3WeightP2 = sp3;
        sprite4WeightP2 = sp4;
        UpdateWeightsTextP2();
    }

    public void reRollP1()
    {
        bool hasMoney = money.SpendMoneyP1(0f);
        if (hasMoney)
        {
            UpdateWeightsP1(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
        }
    }

    public void reRollP2()
    {
        bool hasMoney = money.SpendMoneyP2(0f);
        if (hasMoney)
        {
            UpdateWeightsP2(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
        }


    }

    private void UpdateWeightsTextP1()
    {
        float totalWeight = sprite1WeightP1 + sprite2WeightP1 + sprite3WeightP1 + sprite4WeightP1;
        weightsTextP1.text =
                           $"{(sprite1WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"{(sprite2WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"{(sprite3WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"{(sprite4WeightP1 / totalWeight * 100f):F1}%";
    }
    private void UpdateWeightsTextP2()
    {
        float totalWeight = sprite1WeightP2 + sprite2WeightP2 + sprite3WeightP2 + sprite4WeightP2;

        weightsTextP2.text =
                             $"{(sprite1WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"{(sprite2WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"{(sprite3WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"{(sprite4WeightP2 / totalWeight * 100f):F1}%";
    }


    public float CalculateSideWinProbability()
    {
        float[] unitWeightsSide1 = { sprite1WeightP1, sprite2WeightP1, sprite3WeightP1, sprite4WeightP1 };
        float[] unitWeightsSide2 = { sprite1WeightP2, sprite2WeightP2, sprite3WeightP2, sprite4WeightP2 };
        float totalSide1Probability = 0f;
        float totalSide2Probability = 0f;

        for (int i = 0; i < unitClasses.Length; i++)
        {
            UnitBaseStats unitSide1 = unitClasses[i];
            UnitBaseStats unitSide2 = unitClasses[i];

            // Calculate the win probability for the current unit type (side 1 vs side 2)
            float winProbability = UnitBaseStats.CalculateCombatOutcome(unitSide1, unitSide2);

            // Apply the weights for each unit type on both sides
            totalSide1Probability += winProbability * unitWeightsSide1[i];
            totalSide2Probability += (1 - winProbability) * unitWeightsSide2[i];
        }

        // Normalize the probabilities so they sum to 1
        float totalProbability = totalSide1Probability + totalSide2Probability;
        float side1WinProbability = totalSide1Probability / totalProbability;
        float side2WinProbability = totalSide2Probability / totalProbability;
        // Return P1 probability
        return side1WinProbability;
    }
}
