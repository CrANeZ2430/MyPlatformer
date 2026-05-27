using UnityEngine;

public class HealthBonus : CollectibleItem
{
    [SerializeField] private Sprite healthParticlesSprite;
    [SerializeField] private int healthToRestore = 10;

    protected override void CollectItem(PlayerController player)
    {
        if (player.health != player.maxHealth)
        {
            player.health += healthToRestore;
            player.UIController.ChangeHealthBar(player.health, player.maxHealth);
            DestroyBonus(healthParticlesSprite);
        }
    }
}