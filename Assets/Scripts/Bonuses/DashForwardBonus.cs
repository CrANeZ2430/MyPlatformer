using System.Collections;
using UnityEngine;

public class DashForwardBonus : CollectibleItem
{
    [SerializeField] private int dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Sprite dashForwardParticlesSprite;

    private int dashDir = 1;

    protected override void CollectItem(GameObject player)
    {
        var coroutineRunner = player.GetComponent<ICoroutineRunner>();
        var trailRenderer = player.GetComponent<TrailRenderer>();
        var rb = player.GetComponent<Rigidbody2D>();
        var dashController = player.GetComponent<IDashable>();
        var movementController = player.GetComponent<IMoveable>();
        coroutineRunner.ExecuteCoroutine(Dash(rb, trailRenderer, dashController, movementController));
        DestroyBonus(dashForwardParticlesSprite);
    }

    private IEnumerator Dash(Rigidbody2D rb, 
        TrailRenderer trailRenderer, 
        IDashable dashController, 
        IMoveable movementController)
    {
        dashController.ChangeIsDashing(isDashing:true);
        var originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
        movementController.ChangeObjDir(dashDir);
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        dashController.ChangeIsDashing(isDashing:false);
    }
}
