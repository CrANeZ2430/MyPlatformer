using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private float platformSpeed;
    [SerializeField] private string playerTag;

    private int i;

    void Update()
    {
        var hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y-0.25f), new Vector2(1.9f, 0.01f), 0f);

        if (hit != null && hit.CompareTag(playerTag) && hit.GetComponent<PlayerController>().IsGrounded)
        {
            var player = hit.GetComponent<PlayerController>();
            player.Damage(true);
            i++;
            if (i == positions.Length)
                i = 0;
            
        }

        if (Vector2.Distance(transform.position, positions[i].position) < 0.01f)
        {
            i++;
            if (i == positions.Length)
                i = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, 
                                positions[i].position, Time.deltaTime * platformSpeed);
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
