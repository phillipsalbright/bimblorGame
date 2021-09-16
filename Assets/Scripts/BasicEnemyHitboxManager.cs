using UnityEngine;

/**
 * Manages the hitbox changes for the basic frog enemy.
 */
public class BasicEnemyHitboxManager : MonoBehaviour
{
    /** Different frames of colliders */
    public PolygonCollider2D idle;
    public PolygonCollider2D jumping;

    /** All hitboxes placed here for access within the start method */
    private PolygonCollider2D[] colliders;

    /** Collider attached to the enemy */
    public PolygonCollider2D hitBox;

    public enum currentHitBox
    {
        idle,
        jumping,
    }

    // Start is called before the first frame update
    void Start()
    {
        colliders = new PolygonCollider2D[] { idle, jumping };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHitBox(currentHitBox val)
    {
        hitBox.SetPath(0, colliders[(int)val].GetPath(0));
    }
}
