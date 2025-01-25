using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeightedSpriteCreatorAndMover : MonoBehaviour
{
    private Coroutine moveCoroutine;
    // The Canvas where the sprite will be placed (if it's a UI element)
    public Canvas canvas;

    public GameObject[] predefinedObjectsP1;
    public GameObject[] predefinedObjectsP2;

    public TextMeshProUGUI weightsTextP1;
    public TextMeshProUGUI weightsTextP2;

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

    // Castles
    public GameObject CastleP1;
    public GameObject CastleP2;

    private MoneyManager money;

    private void Start()
    {
        // Automatically start animation when the GameObject is created
        money = GetComponent<MoneyManager>();
        UpdateWeightsTextP2();
        UpdateWeightsTextP1();

    }



    public void sendUnitP1()
    {
        bool hasMoney = money.SpendMoneyP1(10f);
        if (hasMoney)
        {
            CreateAndMoveRandomSprite(CastleP1, CastleP2, sprite1WeightP1, sprite2WeightP1, sprite3WeightP1, sprite4WeightP1, predefinedObjectsP1, "1", "Team 1");
        }
    }

    public void sendUnitP2()
    {
        bool hasMoney = money.SpendMoneyP2(10f);
        if (hasMoney)
        {
            CreateAndMoveRandomSprite(CastleP2, CastleP1, sprite1WeightP2, sprite2WeightP2, sprite3WeightP2, sprite4WeightP2, predefinedObjectsP2, "2", "Team 2");
        }
    }
    // This method will be called by the button OnClick to create a random sprite and move it
    public void CreateAndMoveRandomSprite(GameObject ownCastle, GameObject enemyCastle, float sp1, float sp2, float sp3, float sp4, GameObject[] predefinedObjects, string team, string tag)
    {
        // Weights array for weighted random selection
        float[] weights = { sp1, sp2, sp3, sp4 };

        // Choose a random index based on weights
        int randomIndex = GetRandomIndexByWeight(weights);

        GameObject spriteObject = Instantiate(predefinedObjects[randomIndex]);

        // Optional: Set the name for clarity in the hierarchy
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


        // Start moving the sprite towards the target
        if (controller != null)
        {
            controller.StartMovingSprite(spriteObject, enemyCastle, 10f);
        }
    }



    // Helper method to select a random index based on the weights
    private int GetRandomIndexByWeight(float[] weights)
    {
        // Calculate the total sum of all weights
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // Pick a random value within the total weight range
        float randomValue = Random.Range(0f, totalWeight);

        // Iterate through the weights and select the correct index
        float cumulativeWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;  // Return the index of the selected sprite
            }
        }

        return weights.Length - 1;  // Fallback in case of rounding errors
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
        bool hasMoney = money.SpendMoneyP1(50f);
        if (hasMoney)
        {

            // Define new weights (can be hardcoded or dynamic)
            float newSprite1Weight = Random.Range(1f, 10f);
            float newSprite2Weight = Random.Range(1f, 10f);
            float newSprite3Weight = Random.Range(1f, 10f);
            float newSprite4Weight = Random.Range(1f, 10f);

            // Call the original UpdateWeights method with these values
            UpdateWeightsP1(newSprite1Weight, newSprite2Weight, newSprite3Weight, newSprite4Weight);
        }
    }

    public void reRollP2()
    {
        bool hasMoney = money.SpendMoneyP2(50f);
        if (hasMoney)
        {


            // Generate random weights between 1 and 10 for each sprite
            float newSprite1Weight = Random.Range(1f, 10f);
            float newSprite2Weight = Random.Range(1f, 10f);
            float newSprite3Weight = Random.Range(1f, 10f);
            float newSprite4Weight = Random.Range(1f, 10f);

            // Call the UpdateWeightsP2 method with the new random values
            UpdateWeightsP2(newSprite1Weight, newSprite2Weight, newSprite3Weight, newSprite4Weight);
        }


    }

    private void UpdateWeightsTextP1()
    {
        float totalWeight = sprite1WeightP1 + sprite2WeightP1 + sprite3WeightP1 + sprite4WeightP1;
        weightsTextP1.text =
                           $"S1: {(sprite1WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"S2: {(sprite2WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"S3: {(sprite3WeightP1 / totalWeight * 100f):F1}%\n" +
                           $"S4: {(sprite4WeightP1 / totalWeight * 100f):F1}%";
    }

    private void UpdateWeightsTextP2()
    {
        float totalWeight = sprite1WeightP1 + sprite2WeightP1 + sprite3WeightP1 + sprite4WeightP1;
        weightsTextP2.text =
                             $"S1: {(sprite1WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"S2: {(sprite2WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"S3: {(sprite3WeightP2 / totalWeight * 100f):F1}%\n" +
                             $"S4: {(sprite4WeightP2 / totalWeight * 100f):F1}%";
    }
}
