using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int maxHealth = 3;
    public int currentHealth;
    
    /** The image objects for the health in the scene */
    public Image[] healthImages;

    /** Sprites used for the image objects */
    public Sprite[] healthSprites;

    public void UpdateHearts ()
    {
        currentHealth = this.gameObject.GetComponentInParent<PlayerScript>().health;
        int i = 1;
        foreach (Image image in healthImages)
        {
            if (i <= currentHealth)
            {
                image.sprite = healthSprites[1];
            } else
            {
                image.sprite = healthSprites[0];
            }
            i++;
        }
    }

    /**
     * Used for generating the correct amount of hearts for the max health. May not use this
     * sense the amount of hearts will probably be constant.
     */
    void checkHealthAmount()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (maxHealth <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }
    }

}
