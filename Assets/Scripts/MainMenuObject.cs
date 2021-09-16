using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is connected to a GameObject in the mainmenu scene and contains methods to be
 * called by attached buttons on the menu to load different scenes.
 */
public class MainMenuObject : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
