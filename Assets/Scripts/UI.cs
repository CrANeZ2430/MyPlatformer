using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private int maxHealth, maxMana;
    [SerializeField] private float manaDelay;
    [SerializeField] private string damageTag;

    [SerializeField] private GameObject manaShard;

    [SerializeField] private Sprite fullHeartSprite, halfHeartSprite, noHeartSprite;
    [SerializeField] private Sprite fullManaSprite, noManaSprite;

    [SerializeField] private Image healthSprite1, healthSprite2, healthSprite3;
    [SerializeField] private Image manaSprite1, manaSprite2, manaSprite3;
    [SerializeField] private Image manaCoolDown;

    [SerializeField] private TMP_Text coinsText;

    public int Health { get; private set; }
    public int Mana { get; private set; }
    public int Coins { get; private set; }
    private bool canSpawn = true;
    
    private void Start()
    {
        Health = maxHealth;
        Mana = maxMana;
        coinsText.text = Coins.ToString();
    }

    private void Update()
    {
        SpendMana();
    }

    //Generic Methods
    public void ChangeUIValue(int changeAm, int changeDigit)
    {
        if (changeDigit == 0 && Health != 0)
        {
            Health += changeAm;
            CheckHealth();
        }
        else if (changeDigit == 1 && Mana != 0)
        {
            Mana += changeAm;
            CheckMana();
        }
        else if (changeDigit == 2)
        {
            Coins += changeAm;
            coinsText.text = Coins.ToString();
        }
    }

    private void SetSprite(Image uiImage, Sprite uiSprite)
    {
        uiImage.GetComponent<Image>().sprite = uiSprite;
    }

    private void SetUIBar(Sprite hSprite1, Sprite hSprite2, Sprite hSprite3, bool isHealth)
    {
        if (isHealth)
        {
            SetSprite(healthSprite1, hSprite1);
            SetSprite(healthSprite2, hSprite2);
            SetSprite(healthSprite3, hSprite3);
        }
        else
        {
            SetSprite(manaSprite1, hSprite1);
            SetSprite(manaSprite2, hSprite2);
            SetSprite(manaSprite3, hSprite3);
        }
    }

    //Changing UI bars
    private void CheckHealth()
    {
        switch (Health)
        {
            case 6:
                SetUIBar(fullHeartSprite, fullHeartSprite, fullHeartSprite, true);
                break;
            case 5:
                SetUIBar(fullHeartSprite, fullHeartSprite, halfHeartSprite, true);
                break;
            case 4:
                SetUIBar(fullHeartSprite, fullHeartSprite, noHeartSprite, true);
                break;
            case 3:
                SetUIBar(fullHeartSprite, halfHeartSprite, noHeartSprite, true);
                break;
            case 2:
                SetUIBar(fullHeartSprite, noHeartSprite, noHeartSprite, true);
                break;
            case 1:
                SetUIBar(halfHeartSprite, noHeartSprite, noHeartSprite, true);
                break;
            case 0:
                SetUIBar(noHeartSprite, noHeartSprite, noHeartSprite, true);
                break;

        }
    }

    private void CheckMana()
    {
        switch (Mana)
        {
            case 3:
                SetUIBar(fullManaSprite, fullManaSprite, fullManaSprite, false);
                break;
            case 2:
                SetUIBar(fullManaSprite, fullManaSprite, noManaSprite, false);
                break;
            case 1:
                SetUIBar(fullManaSprite, noManaSprite, noManaSprite, false);
                break;
            case 0:
                SetUIBar(noManaSprite, noManaSprite, noManaSprite, false);
                break;
        }
    }

    //Other Methods
    private void SpendMana()
    {
        if (Input.GetKeyDown(KeyCode.F) && Mana != 0 && canSpawn)
        {
            ChangeUIValue(-1, 1);
            StartCoroutine(SpawnShard());
        }

        ChangeManaCoolDown();
    }

    private IEnumerator SpawnShard()
    {
        Instantiate(manaShard, transform.position, Quaternion.identity);
        canSpawn = false;

        if (Mana == 0)
        {
            canSpawn = true;
            yield break;
        }

        yield return new WaitForSeconds(manaDelay);

        canSpawn = true;
    }

    private void ChangeManaCoolDown()
    {
        if (!canSpawn)
        {
            manaCoolDown.fillAmount += 1f / manaDelay * Time.deltaTime;
        }
        else
        {
            manaCoolDown.fillAmount = 0f;
        }

        if (Mana == 0)
        {
            manaCoolDown.fillAmount = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(damageTag) && Health != 0)
        {
            ChangeUIValue(-1, 0);
        }
    }
}
