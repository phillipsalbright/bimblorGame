using UnityEngine;
using UnityEngine.SceneManagement;

public class TempWinScript : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().RemoveSceneLoadedMethod();
        Destroy(GameObject.FindWithTag("Player"));
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().RemoveSceneLoadedMethod();
        Destroy(GameObject.FindWithTag("Player"));
        SceneManager.LoadScene(0);
    }
}
