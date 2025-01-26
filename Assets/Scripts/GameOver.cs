using UnityEngine;
using UnityEngine.UI; // For UI components
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Reference to the Game Over UI Panel
    public TextMeshProUGUI gameOverText;        // Reference to the Game Over Text (optional)
    public float timeBeforePause = 1f; // Time before pausing after game over (optional)

    // This method is called when the game over condition is met
    public void GameOver(string winner)
    {
        gameOverPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        GameObject winP1 = GameObject.Find("WinP1");
        GameObject winP2 = GameObject.Find("WinP2");

        if (winner == "p1")
        {
            winP2.SetActive(false);
        }
        else
        {
            winP1.SetActive(false);
        }

        // Optionally, you can wait a few seconds before showing the game over screen
        Invoke("ShowGameOverScreen", timeBeforePause);

    }

    // This method shows the Game Over UI and possibly restarts or exits the game
    void ShowGameOverScreen()
    {
        // Show the Game Over overlay
        gameOverPanel.SetActive(true);

        // Optionally, update the text or display more info
        gameOverText.text = "Game Over!";

        // You could also add functionality here to restart or quit the game, e.g.:
        // - Restart button can call RestartGame()
        // - Quit button can call QuitGame()
    }

    // Example function to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        gameOverPanel.SetActive(false); // Hide the game over screen
                                        // Reload the scene or reset variables for a new game (e.g., SceneManager.LoadScene("SceneName"))
        SceneManager.LoadScene("MainScene");
    }

    // Example function to quit the game
    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure time is resumed
        // Quit the application
        Application.Quit();
    }
    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
}
