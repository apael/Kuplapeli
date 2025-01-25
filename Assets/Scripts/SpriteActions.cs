using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSheetAnimator : MonoBehaviour
{
    public Image spriteImage; // The UI Image component
    public Sprite[] frames; // Array of individual sprites from the spritesheet
    public float frameRate = 5f; // Frames per second

    private void Start()
    {
        // Automatically start animation when the GameObject is created
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        StartCoroutine(AnimateSprite());
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
