using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // Required for working with UI components

public class WeightedSpriteCreatorAndMover : MonoBehaviour
{
    // References to the sprites
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;

    // The Canvas where the sprite will be placed (if it's a UI element)
    public Canvas canvas;



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
CreateAndMoveRandomSprite(CastleP1, CastleP2,sprite1WeightP1,sprite2WeightP1,sprite3WeightP1,sprite4WeightP1);
}

       public void sendUnitP2()
{
CreateAndMoveRandomSprite(CastleP2,CastleP1,sprite1WeightP2,sprite2WeightP2,sprite3WeightP2,sprite4WeightP2);
} 
    // This method will be called by the button OnClick to create a random sprite and move it
    public void CreateAndMoveRandomSprite(GameObject ownCastle ,GameObject enemyCastle, float sp1, float sp2, float sp3, float sp4 )
    {
        // Weights array for weighted random selection
        float[] weights = { sp1, sp2, sp3, sp4 };

        // Choose a random index based on weights
        int randomIndex = GetRandomIndexByWeight(weights);

        // Create the sprite object
        GameObject spriteObject = new GameObject("RandomSprite");

        // Add a SpriteRenderer component to the new GameObject
        Image image = spriteObject.AddComponent<Image>();

        // Assign the randomly selected sprite to the SpriteRenderer
        switch (randomIndex)
        {
            case 0: image.sprite = sprite1; break;
            case 1: image.sprite = sprite2; break;
            case 2: image.sprite = sprite3; break;
            case 3: image.sprite = sprite4; break;
        }

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


        // Start moving the sprite towards the target
        StartCoroutine(MoveSpriteToTarget(spriteObject,enemyCastle));
    }

    // Coroutine to move the sprite towards the target position over time
    private IEnumerator MoveSpriteToTarget(GameObject sprite, GameObject enemyCastle )
    {
        float duration = 2f;  // Duration of the movement
        Vector3 startPosition = sprite.transform.position;
        Vector3 targetPosition = enemyCastle.transform.position; // Get target's position
        float timeElapsed = 0f;

        // Move the sprite towards the target position
        while (timeElapsed < duration)
        {
            sprite.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Ensure the sprite reaches the target position exactly
        sprite.transform.position = targetPosition;

        // Destroy the sprite once it reaches the target
        Destroy(sprite);
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
