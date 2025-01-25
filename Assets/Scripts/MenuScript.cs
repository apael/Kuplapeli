using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Optional: Overload method to dynamically set the scene name and load it
    public void ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
