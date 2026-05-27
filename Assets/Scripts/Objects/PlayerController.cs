using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int manaCooldown;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float checkRadius = 0.15f;

    public int health;
    public int mana;
    public int coins;

    public int maxHealth = 50;
    public int maxMana = 50;

    [SerializeField] private LayerMask groundLayer, platformLayer;
    [SerializeField] private GameObject manaShard;

    public Rigidbody2D Rb {get; private set;}
    public UIController UIController {get; private set;}
    public TrailRenderer TrailRenderer {get; private set;}
    public SpriteRenderer SpriteRenderer {get; private set;}
    public bool IsGrounded {get; private set;}
    

    public float playerLastDir = 1f;
    public bool isDashing;
    public bool canSpawnShard = true;

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        UIController = GetComponent<UIController>();
        TrailRenderer = GetComponent<TrailRenderer>();
        SpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.F) && canSpawnShard)
        {
            mana -= 10;
            UIController.ChangeManaBar(mana, maxMana);
            Instantiate(manaShard, transform.position, Quaternion.identity);
            UIController.ManaCooldown(ref canSpawnShard, manaCooldown, mana, () => canSpawnShard = !canSpawnShard);
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // IsGrounded = CheckGround();

            int combinedMask = groundLayer.value | platformLayer.value;
            IsGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, checkRadius, combinedMask);
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
            Rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Rb.linearVelocity.y);

            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                playerLastDir = Input.GetAxisRaw("Horizontal");
            }
            
            //transform.localScale = new Vector3(playerLastDir, 1f, 1f);
            SpriteRenderer.flipX = (playerLastDir < 0f);

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                Jump();
            }
        }
    }

    public void Jump()
    {
        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, jumpForce);
    }

    // private bool CheckGround()
    // {
    //     CapsuleCollider2D cc = GetComponent<CapsuleCollider2D>();
    //     if (cc == null) return false;

    //     // Start at the dynamic mathematical center of your capsule
    //     Vector2 startPoint = cc.bounds.center;
        
    //     // Width is 90% of your collider width, height is a thin slice
    //     Vector2 boxSize = new Vector2(cc.bounds.size.x * 0.9f, 0.1f);
        
    //     // Distance travels exactly half the collider's height + a tiny skin buffer (0.1f)
    //     float castDistance = (cc.bounds.size.y / 2f) + 0.1f;

    //     int combinedMask = groundLayer.value | platformLayer.value;

    //     RaycastHit2D hit = Physics2D.BoxCast(startPoint, boxSize, 0f, Vector2.down, castDistance, combinedMask);

    //     // This green line will now perfectly mirror the physics box reach
    //     Debug.DrawRay(startPoint, Vector2.down * castDistance, Color.green);

    //     return hit.collider != null;
    // }

    public void Damage(bool damagedBYPlatform)
    {
        health -= 10;
        if (!damagedBYPlatform)
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, jumpForce);
        StartCoroutine(BlinkRed());

        UIController.ChangeHealthBar(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damageable")
        {
            Damage(false);
        }
    }

    //for bonuses pickups
    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
