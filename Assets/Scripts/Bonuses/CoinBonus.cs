using UnityEngine;

public class CoinBonus : CollectibleItem
{
    [SerializeField] private Sprite coinParticlesSprite;
    [SerializeField] private int coinsToAdd;

    protected override void CollectItem(GameObject player)
    {
        var resourceController = player.GetComponent<IResourceMutable>();

        resourceController.AddCoins(coinsToAdd);
        DestroyBonus(coinParticlesSprite);
    }
}
