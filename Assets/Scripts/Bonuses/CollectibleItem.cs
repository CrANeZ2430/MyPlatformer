using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        var playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        CollectItem(player, playerRb);
    }

    protected abstract void CollectItem(Player player, Rigidbody2D playerRb);

    protected void DestroyBonus(Sprite particlesSprite, GameObject bonusDestroyedParticles, GameObject bonusObject)
    {
        Destroy(bonusObject);
        bonusDestroyedParticles.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, particlesSprite);
        Instantiate(bonusDestroyedParticles, bonusObject.transform.position, Quaternion.identity);
    }
}
