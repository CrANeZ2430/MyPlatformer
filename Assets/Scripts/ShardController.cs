using UnityEngine;

public class ShardController : MonoBehaviour
{
    [SerializeField] private float shardSpeed;
    [SerializeField] private LayerMask groundLayer;

    private float shardDir;
    private Rigidbody2D shardRb;
    private Player playerComponent;

    private void Awake()
    {
        shardRb = GetComponent<Rigidbody2D>();
        playerComponent = FindObjectOfType<Player>();
    }

    private void Start()
    {
        shardDir = playerComponent.GetPlayerDirection();
        transform.localScale = new Vector3(shardDir, 1f, 1f);
    }

    private void Update()
    {
        shardRb.velocity = new Vector2(shardDir * shardSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int bitmask = 1 << collision.gameObject.layer;

        if (bitmask == groundLayer)
        {
            Destroy(gameObject);
        }
    }
}
