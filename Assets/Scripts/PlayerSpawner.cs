using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    /** player to be spawned */
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            Instantiate(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
