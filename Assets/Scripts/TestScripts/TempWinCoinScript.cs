using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWinCoinScript : MonoBehaviour
{
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerScript>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.transform.Find("HeartCanvas").gameObject.SetActive(false);
            winScreen.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
