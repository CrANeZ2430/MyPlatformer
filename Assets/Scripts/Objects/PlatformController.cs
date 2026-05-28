using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private float platformSpeed;
    [SerializeField] private string playerTag;
    [SerializeField] private LayerMask groundLayer;

    private int currentIndex = 0;
    private bool movingForward = true;

    void Update()
    {
        var hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y-0.25f), new Vector2(1.9f, 0.01f), 0f);

        if (hit != null && hit.CompareTag(playerTag) && hit.GetComponent<PlayerController>().IsGrounded)
        {
            var player = hit.GetComponent<PlayerController>();
            player.Damage(true);
            AdvanceToNextTarget();
        }

        if (hit != null && (groundLayer.value & (1<<hit.gameObject.layer))!= 0)
        {
            AdvanceToNextTarget();
        }

        if (positions == null || positions.Length == 0) return;

        if (Vector2.Distance(transform.position, positions[currentIndex].position) < 0.01f)
        {
            AdvanceToNextTarget();
        }

        transform.position = Vector2.MoveTowards(
            transform.position, 
            positions[currentIndex].position, 
            Time.deltaTime * platformSpeed
        );
    }

    private void AdvanceToNextTarget()
    {
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            collision.transform.SetParent(null);
        }
    }
}
