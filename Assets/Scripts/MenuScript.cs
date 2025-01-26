using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject canvasToHide;

    // Optional: Overload method to dynamically set the scene name and load it
    public void ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
    }
    public void OnStoryButtonClick()
    {
        // Check if the canvas is not already null
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false); // Hide the canvas
            Debug.Log("Canvas Hidden");
        }
    }
    public void OnBackButtonClick()
    {
        // Check if the canvas is not already null
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(true); // Hide the canvas
            Debug.Log("Canvas shown");
        }
    }
}
