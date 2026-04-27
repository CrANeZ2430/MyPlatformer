using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer, platformLayer;

    private float playerLastDir = 1f;
    private Rigidbody2D playerRb;
    private TrailRenderer playerTr;
    private MenuController menuContComponent;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Image dashImage, doubleJumpImage;

    [SerializeField] private Sprite isDashingSprite, notDashingSprite,
                                    isDoubleJumpSprite, notDoubleJump;

    public bool IsDashing {get; private set;}
    private bool canDoubleJump;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerTr = GetComponent<TrailRenderer>();
        menuContComponent = FindObjectOfType<MenuController>();
    }
    
    private void Update()
    {
        if (!menuContComponent.GetMenuOpen())
        {
            Movement();
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
        if (!IsDashing)
        {
            playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerRb.linearVelocity.y);
        }

        transform.localScale = new Vector3(playerLastDir, 1f, 1f);

        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            playerLastDir = Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
        {
            StartCoroutine(Jump());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !CheckGround() && canDoubleJump)
        {
            StartCoroutine(Jump());
        }
    }
    
    public void ChangeDoubleJump()
    {
        canDoubleJump = true;
        SetSprite(doubleJumpImage, isDoubleJumpSprite);
    }

    public bool CheckGround()
    {
        float boxAngle = 0f;
        float boxDist = 1.5f;
        Vector2 boxDir = Vector2.down;
        Vector2 boxSize = new Vector2(0.95f, boxDist);

        bool canJump = Physics2D.BoxCast(transform.position, boxSize, boxAngle, boxDir, boxDist, groundLayer);

        return canJump;
    }

    private void SetSprite(Image uiImage, Sprite uiSprite)
    {
        uiImage.GetComponent<Image>().sprite = uiSprite;
    }

    public IEnumerator Dash(float dashDir)
    {
        IsDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        SetSprite(dashImage, isDashingSprite);

        playerRb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        playerLastDir = dashDir;
        playerTr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        playerTr.emitting = false;
        SetSprite(dashImage, notDashingSprite);
        playerRb.gravityScale = originalGravity;
        IsDashing = false;
    }

    private IEnumerator Jump()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
        float originalScale = playerRb.gravityScale;
        playerRb.gravityScale = 0f;

        yield return null;

        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        playerRb.gravityScale = originalScale;

        canDoubleJump = false;
        SetSprite(doubleJumpImage, notDoubleJump);
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
