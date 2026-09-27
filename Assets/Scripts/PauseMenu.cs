using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isPaused)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        GameManager.Instance.isPaused = false; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false); // Hide the pause menu
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
