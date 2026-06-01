using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TreasureChest : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Reward Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 popForce = new Vector2(0f, 7f);

    [Header("Animation Settings")]
    [SerializeField] private string openTriggerName = "ChestOpened";

    private Animator animator;
    private bool isOpened = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && !isOpened)
        {
            isOpened = true;
            
            animator.SetTrigger(openTriggerName);
        }
    }

    public void SpawnRewardEvent()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No coin prefab assigned!");
            return;
        }

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject spawnedCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D coinRb = spawnedCoin.GetComponent<Rigidbody2D>();
        if (coinRb != null)
        {
            coinRb.linearVelocity = popForce;
        }
    }
}