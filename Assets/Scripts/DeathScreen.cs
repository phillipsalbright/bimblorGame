using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is attached to a canvas for the death screen that pulls up when the player dies.
 */
public class DeathScreen : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
