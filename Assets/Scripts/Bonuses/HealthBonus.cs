using UnityEngine;

public class HealthBonus : CollectibleItem
{
    [SerializeField] private Sprite healthParticlesSprite;
    [SerializeField] private int healthToRestore = 10;

    protected override void CollectItem(GameObject player)
    {
        var resourceChanger = player.GetComponent<IResourceMutable>();

        if (resourceChanger.CurrentHealth < resourceChanger.MaxHealth)
        {
            resourceChanger.AddHealth(healthToRestore);
            DestroyBonus(healthParticlesSprite);
        }
    }
}