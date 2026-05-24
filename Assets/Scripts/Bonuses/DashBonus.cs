using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashBonus : CollectibleItem
{
    public int dashSpeed;
    public float dashDuration;
    public Sprite dashSprite;
    public GameObject bonusDestroyedParticles;

    protected override void CollectItem(PlayerController player)
    {
        player.ExecuteCoroutine(Dash(1, player, player.rb));
        DestroyBonus(dashSprite, bonusDestroyedParticles, gameObject);
    }

    private IEnumerator Dash(float dashDir, PlayerController player, Rigidbody2D playerRb)
    {
        player.isDashing = true;
        var originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;

        playerRb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        player.playerLastDir = dashDir;
        player.trailRenderer.emitting = true;

        yield return new WaitForSecondsRealtime(dashDuration);

        player.trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        player.isDashing = false;
    }
}
