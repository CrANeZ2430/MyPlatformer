using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashBonus : CollectibleItem
{
    public int dashSpeed;
    public float dashDuration;
    public Sprite isDashingSprite, notDashingSprite, dashSprite;
    public Image dashImage;
    public GameObject bonusDestroyedParticles;

    protected override void CollectItem(Player player, Rigidbody2D playerRb)
    {
        player.PickupItem(Dash(1, player, playerRb));
        DestroyBonus(dashSprite, bonusDestroyedParticles, gameObject);
    }

    public IEnumerator Dash(float dashDir, Player player, Rigidbody2D playerRb)
    {
        player.isDashing = true;
        var originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        player.SetSprite(dashImage, isDashingSprite);

        playerRb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        player.playerLastDir = dashDir;
        player.playerTr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        player.playerTr.emitting = false;
        player.SetSprite(dashImage, notDashingSprite);
        playerRb.gravityScale = originalGravity;
        player.isDashing = false;

        Debug.Log("Finished");
    }
}
