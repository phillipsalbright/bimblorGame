using UnityEngine;

/**
 * Script for camera movement. Follows the player and obeys set boundaries.
 */
public class CameraFollow : MonoBehaviour
{

    /** True if the player has been found by the camera */
    private bool playerFound;

    /** player to be followed */
    private Transform player;

    public float smoothSpeed = .125f;

    /** offset from player, set within editor. */
    [SerializeField] private Vector3 offset;

    /** Boundaries for the respective scene, set within editor */
    [SerializeField] private float maxXBound;
    [SerializeField] private float minXBound;
    [SerializeField] private float maxYBound;
    [SerializeField] private float minYBound;

    /**
     * Find the player within the scene after being loaded. Variable definitions make sure that
     * null references are avoided.
     */
    void Start()
    {
        while (player == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                player = GameObject.FindWithTag("Player").transform;
            }
        }
    }

    /**
     * If encountering jittery movement, maybe try LateUpdate (runs after "Update").
     */
    void Update()
    {
        if (player != null)
        {
            Vector3 position = player.position + offset;
            position.x = Mathf.Clamp(position.x, minXBound, maxXBound);
            position.y = Mathf.Clamp(position.y, minYBound, maxYBound);
            transform.position = position;
            return;
        }
    }
}
