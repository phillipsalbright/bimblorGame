using UnityEngine;
using System.Collections;

/**
 * Script for the basic frog enemy.
 */
public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] private int health;

    /** Position of the player in the scene */
    public Transform player;

    /** GameObject at feet of frog to check if touching the ground */
    [SerializeField] private Transform groundCheckTransform;
    private bool playerSpotted;
    
    private float range;
    
    private bool jumped;
    
    private enum State
    {
        Base,
        Jumped,
        Attacking,
    }

    private State state;

    private Rigidbody2D rigidBodyComponent;

    /** Mask of everything frog can jump on (everything but itself) */
    [SerializeField] private LayerMask enemyMask;

    /** Mask of just the player */
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private SpriteRenderer frogSprite;
    
    /** Tongue of frog to spawn in when attacking (condensed tongue prefab) */
    [SerializeField] private GameObject spawnee;

    /** Animator attached to frogSprite SpriteRenderer */
    [SerializeField] private Animator animator;

    private bool onHit = false;

    void Awake()
    {
        health = 3;
        range = 20f;
        rigidBodyComponent = GetComponent<Rigidbody2D>();
        state = State.Base;
        Physics2D.IgnoreLayerCollision(7, 9);
        Physics2D.IgnoreLayerCollision(6, 9);
        animator.SetInteger("state", 0);
        playerSpotted = false;
    }

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

    void FixedUpdate()
    {
        if (player != null)
        {
            if (!playerSpotted)
            {
                if (Vector2.Distance(player.position, transform.position) < range && state != State.Attacking)
                {
                    playerSpotted = true;
                }
            }
            else
            {
                if (state == State.Attacking)
                {
                    return;
                }
                if (Physics2D.OverlapCircleAll(groundCheckTransform.position, .02f, enemyMask).Length != 0)
                {
                    if (Physics2D.OverlapCircleAll(groundCheckTransform.position, .02f, playerMask).Length != 0)
                    {
                        state = State.Base;
                    }
                    switch (state)
                    {
                        case State.Base:
                            float x = player.position.x - transform.position.x;
                            rigidBodyComponent.velocity = Vector2.zero;
                            if (x > 0)
                            {
                                transform.rotation = new Quaternion(0, 180, 0, 0);
                                rigidBodyComponent.AddForce((new Vector2(2, 4)), ForceMode2D.Impulse);
                            }
                            else
                            {
                                transform.rotation = new Quaternion(0, 0, 0, 0);
                                rigidBodyComponent.AddForce((new Vector2(-2, 4)), ForceMode2D.Impulse);
                            }
                            state = State.Jumped;
                            animator.SetInteger("state", 1);
                            break;
                        case State.Jumped:
                            if (!onHit)
                            {
                                rigidBodyComponent.velocity = Vector2.zero;
                            }
                            else
                            {
                                rigidBodyComponent.velocity /= 2;
                            }
                            state = State.Attacking;
                            StartCoroutine(nameof(Attack));
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            float x = transform.position.x - other.transform.position.x;
            if (x <= 0)
            {
                x = -1;
            }
            else
            {
                x = 1;
            }
            //float y = transform.position.y - other.transform.position.y;
            //rigidBodyComponent.velocity = Vector2.zero;
            rigidBodyComponent.velocity = new Vector2(0, rigidBodyComponent.velocity.y);
            rigidBodyComponent.AddForce((new Vector2(x * 1.3f, 0)), ForceMode2D.Impulse);
            if (!onHit)
            {
                health--;
                StartCoroutine(nameof(OnHit));
            }
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Attack()
    {
        animator.SetInteger("state", 0);
        Quaternion rotation;
        float x = player.position.x - transform.position.x;
        if (x > 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            x = 1;
            rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            x = -1;
            rotation = new Quaternion(0, 0, 0, 0);
        }
        yield return new WaitForSeconds(.1f);
        animator.SetInteger("state", 2);
        yield return new WaitForSeconds(.1f);
        Vector3 spawnPos = transform.position;
        spawnPos.y += .075f;
        if (x == 1)
        {
            spawnPos.x += .8f;
        }
        else
        {
            spawnPos.x -= .8f;
        }
        GameObject tongue = Instantiate(spawnee, spawnPos, rotation, transform);
        while (tongue.transform.localScale.x < 1.6f)
        {
            tongue.transform.localScale += new Vector3(.1f, 0, 0);
            tongue.transform.position += new Vector3(x * .05f, 0, 0);
            yield return new WaitForSeconds(.04f);
        }
        while (tongue.transform.localScale.x > .4f)
        {
            tongue.transform.localScale -= new Vector3(.1f, 0, 0);
            tongue.transform.position -= new Vector3(x * .05f, 0, 0);
            yield return new WaitForSeconds(.05f);
        }
        Destroy(tongue);
        animator.SetInteger("state", 0);
        yield return new WaitForSeconds(.3f);
        state = State.Base;
    }

    IEnumerator OnHit()
    {
        onHit = true;
        StartCoroutine(nameof(FlashingAnim));
        yield return new WaitForSeconds(2);
        onHit = false;
    }

    IEnumerator FlashingAnim()
    {
        frogSprite.color = new Color(1, 0, 0, .7f);
        yield return new WaitForSeconds(.5f);
        while (onHit)
        {
            frogSprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.1f);
            frogSprite.color = new Color(1, 1, 1, .5f);
            yield return new WaitForSeconds(.1f);
        }
        frogSprite.color = new Color(1, 1, 1, 1);
    }
}
