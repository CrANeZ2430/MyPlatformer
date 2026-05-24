using UnityEngine;
using UnityEngine.UI;

public class DoubleJumpBonus : CollectibleItem
{
    public Sprite isDoubleJumpSprite, notDoubleJump;
    public Image doubleJumpImage;

    protected override void CollectItem(PlayerController player)
    {
        player.Jump();
        Destroy(gameObject);
    }
}
