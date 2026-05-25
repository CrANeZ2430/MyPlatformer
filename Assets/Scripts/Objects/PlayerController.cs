using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int manaCooldown;

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
        //canSpawnShard = true;
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.F) && canSpawnShard)
        {
            SpendMana();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            IsGrounded = CheckGround();
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

    private bool CheckGround()
    {
        var boxAngle = 0f;
        var boxDist = 1.5f;
        Vector2 boxDir = Vector2.down;
        Vector2 boxSize = new Vector2(0.95f, boxDist);

        return Physics2D.BoxCast(transform.position, boxSize, boxAngle, boxDir, boxDist, groundLayer);
    }

    private void SpendMana()
    {
        mana -= 10;
        UIController.ChangeManaBar(mana, maxMana);
        Instantiate(manaShard, transform.position, Quaternion.identity);
        UIController.ManaCooldown(ref canSpawnShard, manaCooldown, mana, () => canSpawnShard = !canSpawnShard);
    }

    public void Damage()
    {
        health -= 10;
        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, jumpForce);
        StartCoroutine(BlinkRed());

        UIController.ChangeHealthBar(health, maxHealth);
        Debug.Log(health);
        Debug.Log(maxHealth);

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
            Damage();
        }
    }

    //for bonuses pickups
    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int bitmask = 1 << collision.gameObject.layer;

        if (bitmask == platformLayer)
        {
            transform.SetParent(collision.transform, true);
        }

        //Debug.Log(Convert.ToString(bitmask, 2).PadLeft(32, '0'));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int bitmask = 1 << collision.gameObject.layer;

        if (bitmask == platformLayer)
        {
            transform.SetParent(null, true);
        }

    }
}
