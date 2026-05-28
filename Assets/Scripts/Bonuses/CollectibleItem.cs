using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    [SerializeField] private GameObject bonusDestroyedParticles;

    void OnTriggerEnter2D(Collider2D collision)
    {
        CollectItem(collision.gameObject);
    }

    protected abstract void CollectItem(GameObject player);

    protected void DestroyBonus(Sprite particlesSprite)
    {
        Destroy(gameObject);
        bonusDestroyedParticles.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, particlesSprite);
        Instantiate(bonusDestroyedParticles, gameObject.transform.position, Quaternion.identity);
    }
}
