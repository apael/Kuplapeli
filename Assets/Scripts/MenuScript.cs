using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject[] backgroundImages;
    // Optional: Overload method to dynamically set the scene name and load it
    public void ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
    }
    // Method to activate a specific background and hide the rest
    public void ShowBackground(int index)
    {
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            backgroundImages[i].SetActive(i == index); // Only activate the selected background
        }
    }

    // Button events for each background
    public void OnStoryButtonClick()
    {
        ShowBackground(0); // Show the first background (Story)
    }

    public void OnHowToButtonClick()
    {
        ShowBackground(1); // Show the second background (How To)
    }

    public void OnCreditButtonClick()
    {
        ShowBackground(2); // Show the third background (Credit)
    }

    public void OnBackButtonClick()
    {
        ShowBackground(3); // Show the fourth background (Main Menu or default)
    }
    
}
