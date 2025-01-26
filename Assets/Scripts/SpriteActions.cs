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

    private GameObject targetCastle;

    public Transform targetEnemy;
    private MoneyManager moneyMaker;
    private SpriteCreator spriteCreator;


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
        moneyMaker = FindFirstObjectByType<MoneyManager>(); 
        spriteCreator = FindFirstObjectByType<SpriteCreator>(); 
        // Automatically start animation when the GameObject is created
        PlayAnimation();

    }

    public void PlayAnimation()
    {
        StartCoroutine(AnimateSprite());
    }
    private IEnumerator MoveSpriteToTarget(GameObject sprite, GameObject enemyCastle, float speed)
    {
        Vector3 startPosition = sprite.transform.position;
        Vector3 targetPosition = enemyCastle.transform.position; // Default to enemyCastle

        float totalDistance = Vector3.Distance(startPosition, targetPosition); // Calculate total distance to the target
        float traveledDistance = 0f; // Tracks how far the sprite has moved
        Vector3 lastPosition = startPosition; // To track position updates for calculating traveled distance

        while (sprite.transform.position != targetCastle.transform.position)
        {
            float moveStep = speed * Time.deltaTime; // Movement step per frame
            if (isStopped)
            {
                yield return new WaitWhile(() => isStopped); // Wait until `isStopped` is false
            }

            if (targetEnemy != null)
            {
                moveStep = speed / 3 * Time.deltaTime;
                targetPosition = targetEnemy.position;
            }
            else
            {
                targetPosition = targetCastle.transform.position;
            }

            // Move the sprite towards the target position
            sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, moveStep);

            // Calculate the traveled distance
            traveledDistance += Vector3.Distance(lastPosition, sprite.transform.position);
            lastPosition = sprite.transform.position;
            if (Vector3.Distance(sprite.transform.position, targetCastle.transform.position) < 25f)
            {
                break; // Exit loop when very close to the target
            }

            yield return null; // Wait until next frame
        }
        handleDeath(sprite);
    }


    public void handleDeath(GameObject sprite)
    {

        UnitBaseStats baseStats = sprite.GetComponent<UnitBaseStats>();
        CastleStats castleStats = targetCastle.GetComponent<CastleStats>();

        if (baseStats.team.Equals("1"))
        {
            spriteCreator.modifyCountP1(-1);
        }
        else
        {
            spriteCreator.modifyCountP2(-1);
        }
        castleStats.setHp(10);
        Destroy(sprite);
    }

    public void GameOver(CastleStats castleStats)
    {

    }


    public void StartMovingSprite(GameObject sprite, GameObject enemyCastle, float duration = 10f)
    {
        targetCastle = enemyCastle;
        StartCoroutine(MoveSpriteToTarget(sprite, enemyCastle, duration));
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
