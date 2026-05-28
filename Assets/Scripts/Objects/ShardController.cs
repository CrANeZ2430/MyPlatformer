using UnityEngine;

public class ShardController : MonoBehaviour
{
    [SerializeField] private float shardSpeed;
    [SerializeField] private LayerMask groundLayer, platformLayer;

    private float shardDir;
    private Rigidbody2D shardRb;
    private PlayerController playerComponent;

    private void Awake()
    {
        shardRb = GetComponent<Rigidbody2D>();
        playerComponent = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        shardDir = playerComponent.GetPlayerDirection();
        transform.localScale = new Vector3(shardDir, 1f, 1f);
    }

    private void Update()
    {
        shardRb.linearVelocity = new Vector2(shardDir * shardSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int bitmask = 1 << collision.gameObject.layer;
        int combinedMask = groundLayer.value | platformLayer.value;

        if ((bitmask & combinedMask) != 0)
        {
            Destroy(gameObject);
        }
    }
}
