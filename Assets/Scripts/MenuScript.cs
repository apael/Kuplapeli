using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject StoryToHide;
    public GameObject HowToHide;
    // Optional: Overload method to dynamically set the scene name and load it
    public void ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
    }
    public void OnStoryButtonClick()
    {
        // Check if the canvas is not already null
        if (StoryToHide != null)
        {
            StoryToHide.SetActive(false); // Hide the canvas
        }
    }
    public void OnBackButtonClick()
    {
        // Check if the canvas is not already null
        if (StoryToHide != null)
        {
            StoryToHide.SetActive(true); // Hide the canvas
        }
    }
    public void OnHowToButtonClick()
    {
        // Check if the canvas is not already null
        if (HowToHide != null)
        {
            StoryToHide.SetActive(false);
            HowToHide.SetActive(false); // Hide the canvas
            
        }
    }
    public void OnHowToBackButtonClick()
    {
        // Check if the canvas is not already null
        if (HowToHide != null)
        {
            HowToHide.SetActive(true); // Hide the canvas
            StoryToHide.SetActive(true);
        }
    }
}
