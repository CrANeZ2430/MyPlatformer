using UnityEngine;

public class ManaBonus : CollectibleItem
{
    [SerializeField] private Sprite manaParticlesSprite;
    [SerializeField] private int manaToRestore = 10;

    protected override void CollectItem(GameObject player)
    {
        var resourceChanger = player.GetComponent<IResourceMutable>();

        if (resourceChanger.CurrentMana < resourceChanger.MaxMana)
        {
            resourceChanger.AddMana(manaToRestore);
            DestroyBonus(manaParticlesSprite);
        }
    }
}