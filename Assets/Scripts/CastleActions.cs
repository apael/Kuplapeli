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

    // The target position the sprite should move towards

        public GameObject targetObject;

    // Weights for each sprite (higher values mean higher chance of selection)
    public float sprite1Weight = 1f;
    public float sprite2Weight = 2f;
    public float sprite3Weight = 3f;
    public float sprite4Weight = 4f;
    public Vector3 specificStartPosition = new Vector3(0f, 0f, 0f); // Default value (can be changed in Inspector)

    // This method will be called by the button OnClick to create a random sprite and move it
    public void CreateAndMoveRandomSprite()
    {
        // Weights array for weighted random selection
        float[] weights = { sprite1Weight, sprite2Weight, sprite3Weight, sprite4Weight };

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

        // Position the sprite at a random location
        spriteObject.transform.position = specificStartPosition;

        // Optional: If working with UI, set the parent to the Canvas
        spriteObject.transform.SetParent(canvas.transform, false);

        // Start moving the sprite towards the target
        StartCoroutine(MoveSpriteToTarget(spriteObject));
    }

    // Coroutine to move the sprite towards the target position over time
    private IEnumerator MoveSpriteToTarget(GameObject sprite)
    {
        float duration = 2f;  // Duration of the movement
        Vector3 startPosition = sprite.transform.position;
        Vector3 targetPosition = targetObject.transform.position; // Get target's position
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
    public void UpdateWeights(float newSprite1Weight, float newSprite2Weight, float newSprite3Weight, float newSprite4Weight)
    {
        sprite1Weight = newSprite1Weight;
        sprite2Weight = newSprite2Weight;
        sprite3Weight = newSprite3Weight;
        sprite4Weight = newSprite4Weight;

        // Log to check the new weight values
        Debug.Log($"Updated weights: Sprite1: {sprite1Weight}, Sprite2: {sprite2Weight}, Sprite3: {sprite3Weight}, Sprite4: {sprite4Weight}");
    }

    public void UpdateWeightsButtonClick()
{
    // Define new weights (can be hardcoded or dynamic)
    float newSprite1Weight = 1f;
    float newSprite2Weight = 3f;
    float newSprite3Weight = 5f;
    float newSprite4Weight = 2f;

    // Call the original UpdateWeights method with these values
    UpdateWeights(newSprite1Weight, newSprite2Weight, newSprite3Weight, newSprite4Weight);
}

}
