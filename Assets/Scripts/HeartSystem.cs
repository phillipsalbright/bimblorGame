using UnityEngine;
using UnityEngine.UI;

/**
 * Class for displaying player health. Connected to Canvas that is child of the Player object.
 */
public class HeartSystem : MonoBehaviour
{
    /** Image used for displaying player health */
    public Image healthImage;

    /** Sprites used for displaying player health */
    public Sprite[] healthSprites;

    public void UpdateHeart()
    {
        int currentHealth = this.gameObject.GetComponentInParent<PlayerScript>().health;
        if (currentHealth > 0 && currentHealth <= 3)
        {
            healthImage.sprite = healthSprites[currentHealth - 1];
        }
    }
}
