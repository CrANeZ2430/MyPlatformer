using UnityEngine;

public class ManaBonus : CollectibleItem
{
    [SerializeField] private Sprite manaParticlesSprite;
    [SerializeField] private int manaToRestore = 10;

    protected override void CollectItem(PlayerController player)
    {
        if (player.mana != player.maxMana)
        {
            player.mana += manaToRestore;
            player.UIController.ChangeManaBar(player.mana, player.maxMana);
            DestroyBonus(manaParticlesSprite, gameObject);
        }
    }
}