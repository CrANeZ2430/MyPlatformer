using UnityEngine;

public class CoinBonus : CollectibleItem
{
    [SerializeField] private Sprite coinParticlesSprite;

    protected override void CollectItem(PlayerController player)
    {
        player.coins++;
        player.UIController.UpdateCoins(player.coins);
        DestroyBonus(coinParticlesSprite);
    }
}
