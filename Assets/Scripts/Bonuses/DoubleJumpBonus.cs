using UnityEngine;

public class DoubleJumpBonus : CollectibleItem
{
    [SerializeField] private Sprite doubleJumpParticlesSprite;

    protected override void CollectItem(PlayerController player)
    {
        player.Jump();
        DestroyBonus(doubleJumpParticlesSprite);
    }
}
