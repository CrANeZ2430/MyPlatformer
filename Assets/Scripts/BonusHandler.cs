using UnityEngine;

public class BonusHandler : MonoBehaviour
{
    [SerializeField] private string healthBonus, manaBonus, jumpBonus, dashBonus1, dashBonus2, coinBonus;
    [SerializeField] private GameObject bonusDestroyedParticles;
    [SerializeField] private Sprite healthSprite, manaSprite, jumpSprite, dash1Sprite, dash2Sprite, coinSprite;

    private Player playerComponent;
    private UI uiComponent;

    private void Awake()
    {
        playerComponent = GetComponent<Player>();
        uiComponent = GetComponent<UI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(healthBonus))
        {
            if (uiComponent.Health != 6)
            {
                uiComponent.ChangeUIValue(1, 0);
                DestroyBonus(collision, healthSprite);
            }
        }
        else if(collision.CompareTag(manaBonus))
        {
            if (uiComponent.Mana != 3)
            {
                uiComponent.ChangeUIValue(1, 1);
                DestroyBonus(collision, manaSprite);
            }
        }
        else if (collision.CompareTag(jumpBonus))
        {
            playerComponent.ChangeDoubleJump();
            DestroyBonus(collision, jumpSprite);
        }
        else if (collision.CompareTag(dashBonus1))
        {
            if(!playerComponent.IsDashing)
            {
                StartCoroutine(playerComponent.Dash(1f));
                DestroyBonus(collision, dash1Sprite);
            }
        }
        else if (collision.CompareTag(dashBonus2))
        {
            if (!playerComponent.IsDashing)
            {
                StartCoroutine(playerComponent.Dash(-1f));
                DestroyBonus(collision, dash2Sprite);
            }
        }
        else if (collision.CompareTag(coinBonus))
        {
            uiComponent.ChangeUIValue(1, 2);
            DestroyBonus(collision, coinSprite);
        }
    }

    private void DestroyBonus(Collider2D collision, Sprite particlesSprite)
    {
        Destroy(collision.gameObject);
        bonusDestroyedParticles.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, particlesSprite);
        Instantiate(bonusDestroyedParticles, collision.transform.position, Quaternion.identity);
    }
}
