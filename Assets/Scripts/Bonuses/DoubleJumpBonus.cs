using UnityEngine;

public class DoubleJumpBonus : CollectibleItem
{
    [SerializeField] private Sprite doubleJumpParticlesSprite;

    protected override void CollectItem(GameObject player)
    {
        var movementChanger = player.GetComponent<IMoveable>();
        movementChanger.Jump();
        DestroyBonus(doubleJumpParticlesSprite);
    }
}
