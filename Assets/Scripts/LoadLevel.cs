using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is used on objects that act as level loading triggers for the player.
 */
public class LoadLevel : MonoBehaviour
{
    /** Level for this level loader to load */
    [SerializeField] private int iLevelToLoad;

    //public string sLevelToLoad;

    //public bool useIntegerToLoadLevel = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.layer == 3)
        {
            LoadScene();
        }

    }

    void LoadScene()
    {
        SceneManager.LoadScene(iLevelToLoad);
    }
}
