using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip soundClip)
    {
        if (soundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}
