using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyCooldown;
    [SerializeField] private string playerTag;
    //[SerializeField] private LayerMask groundLayer;

    private int currentIndex = 0;
    private bool movingForward = true;
    private bool canMove = true;

    void Start()
    {
        if (positions != null && positions.Length > 0)
        {
            FlipFacingDirection();
        }
    }

    void Update()
    {
        // var hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y-0.25f), new Vector2(1.9f, 0.01f), 0f);

        // if (hit != null && hit.CompareTag(playerTag) && hit.GetComponent<IMoveable>().IsGrounded)
        // {
        //     var player = hit.GetComponent<IDamageable>();
        //     player.Damage(true);
        //     ChangeTarget();
        // }

        // if (hit != null && (groundLayer.value & (1<<hit.gameObject.layer))!= 0)
        // {
        //     ChangeTarget();
        // }

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
}
