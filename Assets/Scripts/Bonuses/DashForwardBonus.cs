using System.Collections;
using UnityEngine;

public class DashForwardBonus : CollectibleItem
{
    [SerializeField] private int dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Sprite dashForwardParticlesSprite;

    private int dashDir = 1;

    protected override void CollectItem(PlayerController player)
    {
        player.ExecuteCoroutine(Dash(dashDir, player, player.Rb));
        DestroyBonus(dashForwardParticlesSprite);
    }

    private IEnumerator Dash(float dashDir, PlayerController player, Rigidbody2D rb)
    {
        player.isDashing = true;
        var originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        player.playerLastDir = dashDir;
        player.TrailRenderer.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        player.TrailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        player.isDashing = false;
    }
}
