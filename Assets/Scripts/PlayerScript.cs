using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * This class attached to the player prefab contains most of the logic for player movement, health,
 * and collision.
 */
public class PlayerScript : MonoBehaviour
{
    /** True if the jump key was pressed in the last update */
    private bool jumpKeyWasPressed;

    /** Value from horizontal axis in the last update */
    private float horizontalInput;

    /** Animator attached to player body sprite */
    public Animator animator;

    /** Rigidbody2D for the player hitbox */
    private Rigidbody2D rigidbodyComponent;

    public int health;

    /** GameObject near player feet to check if touching the ground */
    public Transform groundCheckTransform;

    /** Mask containing all layers the player should be able to jump off of */
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private SpriteRenderer bodySprite;

    [SerializeField] private SpriteRenderer armSprite;
    
    /** Instance of Script for health bar attached to player */
    private HeartSystem heart;
    
    /** True if player has been "hit" within a certain period of time */
    private bool onHit;

    /** Script for the death screen */
    public DeathScreen DeathScreen;

    /** true if player is dead */
    public bool dead;

    /** true if the player is currently in the process of turning around */
    private bool turning;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        onHit = false;
        turning = false;
        health = 3;
        heart = this.GetComponentInChildren<Canvas>().GetComponent<HeartSystem>();
        heart.UpdateHeart();
        /** Adds the "MoveToStart" method to be called when a scene is loaded. */
        /** Use SceneManager.sceneLoaded -= MoveToStart if want it to not load when scenes load */
        SceneManager.sceneLoaded += MoveToStart;
    }

    public void RemoveSceneLoadedMethod()
    {
        SceneManager.sceneLoaded -= MoveToStart;
    }

    /**
     * If don't want player to move to start immediately, switch to this.
    void Start()
    {
        SceneManager.sceneLoaded += MoveToStart;
    }
     */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && turning == false)
        {
            turning = true;
            StartCoroutine(nameof(Turn));
        }
        horizontalInput = Input.GetAxis("Horizontal");
        if (health <= 0)
        {
            Death();
        }
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(.3f);
        Vector3 rot = transform.rotation.eulerAngles;
        if (rot.y == 0)
        {
            rot.y += 180;
        } else
        {
            rot.y -= 180;
        }
        transform.rotation = Quaternion.Euler(rot);
        yield return new WaitForSeconds(1f);
        turning = false;
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(horizontalInput * 3f, rigidbodyComponent.velocity.y);
        Vector2 m_Velocity = Vector2.zero;
        // slower movement if hit recently
        if (!onHit)
        {
            rigidbodyComponent.velocity = Vector2.SmoothDamp(rigidbodyComponent.velocity, targetVelocity, ref m_Velocity, .05f);
        } else
        {
            rigidbodyComponent.velocity = Vector2.SmoothDamp(rigidbodyComponent.velocity, targetVelocity, ref m_Velocity, .17f);
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput * 2));
        // Check if player is touching the ground and return if they are not. Continue if they are
        if (Physics2D.OverlapCircleAll(groundCheckTransform.position, .03f, playerMask).Length == 0)
        {
            animator.SetBool("groundCheck", false);
            jumpKeyWasPressed = false;
            return;
        } else
        {
            animator.SetBool("groundCheck", true);
        }
        if (jumpKeyWasPressed)
        {
            float jumpPower = 8f;
            rigidbodyComponent.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpKeyWasPressed = false;
        }
    }

    public void Death()
    {
        if (dead != true)
        {
            dead = true;
            SceneManager.sceneLoaded -= MoveToStart;
            Transform m_DestroyOnLoadGO = (new GameObject("DestroyOnLoad")).transform;
            transform.SetParent(m_DestroyOnLoadGO);
            DeathScreen.Setup();
            transform.Find("DeathScreen").SetParent(null);
            transform.Find("EventSystem").SetParent(null);
            transform.Find("HeartCanvas").gameObject.SetActive(false);
            StartCoroutine(nameof(DeathHelper));
        }
    }

    IEnumerator DeathHelper()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            this.Death();
        }
    }

    /**
     * On collision with something that hurts the player, if the onHit bool is false then health is
     * decremented. A force is applied to the player calculated in relation to collider location.
     * A smaller force is applied if the player was recently hit.
     */
    private void OnCollisionEnter2D(Collision2D other)
    {
        int layerNum = other.gameObject.layer;
        if (layerNum == 7 || layerNum == 9)
        {
            float x = transform.position.x - other.transform.position.x;
            if (x <= 0)
            {
                x = -1;
            } else
            {
                x = 1;
            }
            float y = transform.position.y - other.transform.position.y;
            rigidbodyComponent.velocity = Vector2.zero;
            if (!onHit)
            {
                health--;
                rigidbodyComponent.AddForce((new Vector2(x * 4, y * 3)), ForceMode2D.Impulse);
                heart.UpdateHeart();
                StartCoroutine(nameof(OnHit));
            } else
            {
                rigidbodyComponent.AddForce((new Vector2(x, y) * 2.1f), ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator OnHit()
    {
        onHit = true;
        StartCoroutine(nameof(FlashingAnim));
        yield return new WaitForSeconds(2);
        onHit = false;
    }

    /**
     * Coroutine for animation while the OnHit coroutine is running 
     */
    IEnumerator FlashingAnim()
    {
        bodySprite.color = new Color(1, 0, 0, .7f);
        armSprite.color = new Color(1, 0, 0, .7f);
        yield return new WaitForSeconds(.5f);
        while (onHit)
        {
            bodySprite.color = new Color(1, 1, 1, 1);
            armSprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.1f);
            bodySprite.color = new Color(1, 1, 1, .5f);
            armSprite.color = new Color(1, 1, 1, .5f);
            yield return new WaitForSeconds(.1f);
        }
        bodySprite.color = new Color(1, 1, 1, 1);
        armSprite.color = new Color(1, 1, 1, 1);
    }

    public void MoveToStart(Scene scene, LoadSceneMode mode)
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

}
