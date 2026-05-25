using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    [SerializeField] private GameObject bonusDestroyedParticles;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();

        CollectItem(player);
    }

    protected abstract void CollectItem(PlayerController player);

    protected void DestroyBonus(Sprite particlesSprite, GameObject bonusObject)
    {
        Destroy(bonusObject);
        bonusDestroyedParticles.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, particlesSprite);
        Instantiate(bonusDestroyedParticles, bonusObject.transform.position, Quaternion.identity);
    }
}
