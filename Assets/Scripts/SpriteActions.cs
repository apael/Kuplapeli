using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSheetAnimator : MonoBehaviour
{
    public Image spriteImage; // The UI Image component
    public Sprite[] frames; // Array of individual sprites from the spritesheet
    public float frameRate = 5f; // Frames per second

    private bool isStopped = false;

    private Coroutine moveCoroutine;

    // Assign the coroutine from the movement manager
    public void SetMoveCoroutine(Coroutine coroutine)
    {
        moveCoroutine = coroutine;
    }

    // Method to stop the movement
    public void StopMovement()
    {
        isStopped = true;
    }
    public void ContinueMovement()
    {
        isStopped = false;
    }
    private void Start()
    {
        // Automatically start animation when the GameObject is created
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        StartCoroutine(AnimateSprite());
    }

    private IEnumerator MoveSpriteToTarget(GameObject sprite, GameObject enemyCastle, float speed)
    {
        if (sprite == null || enemyCastle == null)
        {
            Debug.LogWarning("Sprite or enemyCastle is null. Aborting movement.");
            yield break;
        }

        Vector3 startPosition = sprite.transform.position;
        Vector3 targetPosition = enemyCastle.transform.position;
        float totalDistance = Vector3.Distance(startPosition, targetPosition);
        float traveledDistance = 0f; // Tracks how far the sprite has moved
        Vector3 lastPosition = startPosition; // To handle pause/resume

        while (traveledDistance < totalDistance -1f)
        {

            if (isStopped)
            {
                Debug.Log("Movement paused!");
                yield return new WaitWhile(() => isStopped); // Wait until `isStopped` is false
                Debug.Log("Movement resumed!");
            }

            if (sprite == null || !sprite.activeInHierarchy)
            {
                Debug.LogWarning("Sprite destroyed or deactivated. Aborting movement.");
                yield break;
            }

            // Calculate movement for this frame
            float moveStep = speed * 10 * Time.deltaTime; // Adjust movement speed here if needed
            sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, moveStep);

            // Update traveled distance
            traveledDistance += Vector3.Distance(lastPosition, sprite.transform.position);
            lastPosition = sprite.transform.position;

            yield return null;
        }
        sprite.transform.position = targetPosition;
        Destroy(sprite);
    }



    public void StartMovingSprite(GameObject sprite, GameObject enemyCastle, float duration = 10f)
    {
        moveCoroutine = StartCoroutine(MoveSpriteToTarget(sprite, enemyCastle, duration));
        SetMoveCoroutine(moveCoroutine);
    }



    private IEnumerator AnimateSprite()
    {
        int index = 0;

        while (true)
        {
            // Set the current frame
            spriteImage.sprite = frames[index];

            // Wait for the next frame
            yield return new WaitForSeconds(1f / frameRate);

            // Move to the next frame
            index = (index + 1) % frames.Length;
        }
    }
}
