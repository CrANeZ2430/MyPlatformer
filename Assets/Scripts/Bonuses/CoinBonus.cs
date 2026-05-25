using UnityEngine;

public class CoinBonus : CollectibleItem
{
    public Sprite coinParticlesSprite;

    protected override void CollectItem(PlayerController player)
    {
        player.coins++;
        player.uiController.UpdateCoins(player.coins);
        DestroyBonus(coinParticlesSprite, bonusDestroyedParticles, gameObject);
    }
}
