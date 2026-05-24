using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private int health;
    private int mana;

    public int maxHealth = 50;
    public int maxMana = 50;

    public LayerMask groundLayer, platformLayer;
    public Rigidbody2D rb;
    public TrailRenderer trailRenderer;
    public SpriteRenderer spriteRenderer;

    public Image healthBar, manaBar;

    public float playerLastDir = 1f;
    public bool isDashing;
    public bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
    }

    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            isGrounded = CheckGround();
        }
    }

    public float GetPlayerDirection()
    {
        float playerDirection = Input.GetAxisRaw("Horizontal");

        if (playerDirection == 0f)
        {
            playerDirection = playerLastDir;
        }

        return playerDirection;
    }

    private void Movement()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y);
            
            transform.localScale = new Vector3(playerLastDir, 1f, 1f);

            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                playerLastDir = Input.GetAxisRaw("Horizontal");
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private bool CheckGround()
    {
        var boxAngle = 0f;
        var boxDist = 1.5f;
        Vector2 boxDir = Vector2.down;
        Vector2 boxSize = new Vector2(0.95f, boxDist);

        return Physics2D.BoxCast(transform.position, boxSize, boxAngle, boxDir, boxDist, groundLayer);
    }

    //for bonuses pickups
    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damageable")
        {
            Damage();
        }
    }

    public void Damage()
    {
        health -= 10;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        StartCoroutine(BlinkRed());

        if (health <= 0)
        {
            Die();
        }

        healthBar.fillAmount = (float)health / maxHealth;
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     int bitmask = 1 << collision.gameObject.layer;

    //     if (bitmask == platformLayer)
    //     {
    //         transform.SetParent(collision.transform, true);
    //     }

    //     //Debug.Log(Convert.ToString(bitmask, 2).PadLeft(32, '0'));
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     int bitmask = 1 << collision.gameObject.layer;

    //     if (bitmask == platformLayer)
    //     {
    //         transform.SetParent(null, true);
    //     }

    // }
}
