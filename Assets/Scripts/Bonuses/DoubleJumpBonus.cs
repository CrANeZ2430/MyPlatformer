using UnityEngine;

public class DoubleJumpBonus : CollectibleItem
{
    public Sprite doubleJumpSprite;

    protected override void CollectItem(PlayerController player)
    {
        player.Jump();
        DestroyBonus(doubleJumpSprite, bonusDestroyedParticles, gameObject);
    }
}
