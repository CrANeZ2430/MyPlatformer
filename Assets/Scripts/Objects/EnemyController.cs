using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private Transform[] positions;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyCooldown;
    [SerializeField] private string playerTag;
    [SerializeField] private AnimationClip deathAnimation;

    private int currentHealth;
    private int currentIndex = 0;
    private bool movingForward = true;
    private bool canMove = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        if (positions != null && positions.Length > 0)
        {
            FlipFacingDirection();
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (positions == null || positions.Length == 0) return;

        if (Vector2.Distance(transform.position, positions[currentIndex].position) < 0.01f)
        {
            ChangeTarget();
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                positions[currentIndex].position, 
                Time.deltaTime * enemySpeed
                );
        }
    }

    public void Damage(int damage, Action onDamage)
    {
        currentHealth -= damage;

        onDamage?.Invoke();
        StartCoroutine(BlinkRed());
        
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        canMove = false;

        animator.SetBool("isDying", true);
        
        yield return new WaitForSeconds(deathAnimation.length);

        Destroy(gameObject);
    }

    private void ChangeTarget()
    {
        canMove = false;

        if (positions.Length <= 1) return;

        if (currentIndex == positions.Length - 1 && movingForward == true)
        {
            movingForward = false;
        }
        else if (currentIndex == 0 && movingForward == false)
        {
            movingForward = true;
        }

        currentIndex += movingForward ? 1 : -1;

        FlipFacingDirection();

        StartCoroutine(PlatformAwait());
    }

    private IEnumerator PlatformAwait()
    {
        yield return new WaitForSeconds(enemyCooldown);

        canMove = true;
    }

    private void FlipFacingDirection()
    {
        // Check if the target is to the right or left of the enemy
        float direction = positions[currentIndex].position.x - transform.position.x;

        if (direction > 0.01f)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < -0.01f)
        {
            // Face left (negate the X scale)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();

            damageable.Damage(damage:-10, () =>
            {
                var rb = collision.gameObject.GetComponent<Rigidbody2D>();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
            });
        }
    }
}
