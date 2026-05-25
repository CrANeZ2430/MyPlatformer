using System.Collections;
using UnityEngine;

public class DashBonus : CollectibleItem
{
    public int dashSpeed;
    public float dashDuration;
    public Sprite dashSprite;

    private int dashDir = 1;

    protected override void CollectItem(PlayerController player)
    {
        player.ExecuteCoroutine(Dash(dashDir, player, player.rb));
        DestroyBonus(dashSprite, bonusDestroyedParticles, gameObject);
    }

    private IEnumerator Dash(float dashDir, PlayerController player, Rigidbody2D rb)
    {
        player.isDashing = true;
        var originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        player.playerLastDir = dashDir;
        player.trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        player.trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        player.isDashing = false;
    }
}
