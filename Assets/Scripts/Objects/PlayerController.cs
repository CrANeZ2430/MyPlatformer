using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IResourceMutable, ICoroutineRunner, IMoveable, IDashable, IDamageable
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float manaCooldown;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxMana;

    [SerializeField] private LayerMask groundLayer, platformLayer;
    [SerializeField] private GameObject manaShard;
    [SerializeField] private Animator animator;

    private bool canSpawnShard = true;

    private Rigidbody2D rb;
    private UIController uiController;
    private SpriteRenderer spriteRenderer;
    
    public bool IsGrounded {get; private set;}

    public int MaxHealth { get; private set;}
    public int CurrentHealth { get; private set;}
    public int MaxMana { get; private set;}
    public int CurrentMana { get; private set;}
    public int Coins { get; private set;}

    public float ObjLastDir { get; private set; } = 1f;
    public bool IsDashing { get; private set; }
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        uiController =  Object.FindFirstObjectByType<UIController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError($"[PlayerController] No SpriteRenderer found on {gameObject.name} or its children! Sprite flipping will crash.");
        }
    }

    void Start()
    {
        MaxHealth = maxHealth;
        MaxMana = maxMana;
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        Coins = 0;
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.F) && canSpawnShard)
        {
            AddMana(manaAmount:-10);
            uiController.ChangeManaBar(CurrentMana, MaxMana);
            Instantiate(manaShard, transform.position, Quaternion.identity);
            uiController.ManaCooldown(ref canSpawnShard, manaCooldown, CurrentMana, () => canSpawnShard = !canSpawnShard);
        }
    }

    void FixedUpdate()
    {
        if (!IsDashing)
        {
            int combinedMask = groundLayer.value | platformLayer.value;
            IsGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, checkRadius, combinedMask);

            // --- НОВІ РЯДКИ ДЛЯ АНІМАЦІЇ СТРИБКА ТА ПАДІННЯ ---
            if (animator != null)
            {
                animator.SetBool("IsGrounded", IsGrounded);
                animator.SetFloat("yVelocity", rb.linearVelocity.y);
            }

        }
    }

    public float GetPlayerDirection()
    {
        var playerDirection = Input.GetAxisRaw("Horizontal");

        if (playerDirection == 0f)
        {
            playerDirection = ObjLastDir;
        }

        return playerDirection;
    }

    private void Move()
    {
        if (!IsDashing)
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y);

            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                ObjLastDir = Input.GetAxisRaw("Horizontal");
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = (ObjLastDir < 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                Jump();
            }
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void Damage(bool damagedByPlatform)
    {
        AddHealth(healthAmount:-10);

        if (!damagedByPlatform)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        StartCoroutine(BlinkRed());
        uiController.ChangeHealthBar(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damageable")
        {
            Damage(false);
        }
    }

    public void ExecuteCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void AddHealth(int healthAmount)
    {
        CurrentHealth += healthAmount;
        uiController.ChangeHealthBar(CurrentHealth, MaxHealth);
    }

    public void AddMana(int manaAmount)
    {
        CurrentMana += manaAmount;
        uiController.ChangeManaBar(CurrentMana, MaxMana);
    }

    public void AddCoins(int coinsAmount)
    {
        Coins += coinsAmount;
        uiController.UpdateCoins(Coins);
    }

    public void ChangeObjDir(float objDir)
    {
        ObjLastDir = objDir;
    }

    public void ChangeIsDashing(bool isDashing)
    {
        IsDashing = isDashing;
    }
}