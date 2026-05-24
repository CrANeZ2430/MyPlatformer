using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();

        CollectItem(player);
    }

    protected abstract void CollectItem(PlayerController player);

    protected void DestroyBonus(Sprite particlesSprite, GameObject bonusDestroyedParticles, GameObject bonusObject)
    {
        Destroy(bonusObject);
        bonusDestroyedParticles.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, particlesSprite);
        Instantiate(bonusDestroyedParticles, bonusObject.transform.position, Quaternion.identity);
    }
}
