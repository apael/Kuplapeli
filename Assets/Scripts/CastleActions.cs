using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightedSpriteCreatorAndMover : MonoBehaviour
{
    private Coroutine moveCoroutine;
    // The Canvas where the sprite will be placed (if it's a UI element)
    public Canvas canvas;

    public GameObject[] predefinedObjectsP1;
    public GameObject[] predefinedObjectsP2;

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

    public void sendUnitP1()
    {
        CreateAndMoveRandomSprite(CastleP1, CastleP2, sprite1WeightP1, sprite2WeightP1, sprite3WeightP1, sprite4WeightP1, predefinedObjectsP1, "1", "Team 1");
    }

    public void sendUnitP2()
    {
        CreateAndMoveRandomSprite(CastleP2, CastleP1, sprite1WeightP2, sprite2WeightP2, sprite3WeightP2, sprite4WeightP2, predefinedObjectsP2, "2", "Team 2");
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
        castleWorldPosition.y += 2.0f; // You can adjust this value to control how high the sprite spawns

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

    }

    public void UpdateWeightsP2(float sp1, float sp2, float sp3, float sp4)
    {
        sprite1WeightP2 = sp1;
        sprite2WeightP2 = sp2;
        sprite3WeightP2 = sp3;
        sprite4WeightP2 = sp4;

    }

    public void reRollP1()
    {
        // Define new weights (can be hardcoded or dynamic)
        float newSprite1Weight = 1f;
        float newSprite2Weight = 3f;
        float newSprite3Weight = 5f;
        float newSprite4Weight = 2f;

        // Call the original UpdateWeights method with these values
        UpdateWeightsP1(newSprite1Weight, newSprite2Weight, newSprite3Weight, newSprite4Weight);
    }

    public void reRollP2()
    {
        // Define new weights (can be hardcoded or dynamic)
        float newSprite1Weight = 1f;
        float newSprite2Weight = 3f;
        float newSprite3Weight = 5f;
        float newSprite4Weight = 2f;

        // Call the original UpdateWeights method with these values
        UpdateWeightsP2(newSprite1Weight, newSprite2Weight, newSprite3Weight, newSprite4Weight);
    }

}
