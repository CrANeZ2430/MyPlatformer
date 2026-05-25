using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image healthBar, manaBar;
    [SerializeField] private Image manaCooldownImage;
    [SerializeField] private TMP_Text coinsText;

    public void ChangeHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void ChangeManaBar(int currentMana, int maxMana)
    {
        manaBar.fillAmount = (float)currentMana / maxMana;
    }

    public void ManaCooldown(ref bool canSpawnShard, int manaCooldown, int mana, Action onCooldownComplete)
    {
        canSpawnShard = false;

        if (mana == 0)
        {
            manaCooldownImage.fillAmount = 1f;
            return;
        }
        else if (mana > 0)
        {
            StartCoroutine(CooldownRoutine(manaCooldown, onCooldownComplete));
        }
    }

    public IEnumerator CooldownRoutine(int manaCooldown, Action onCooldownComplete)
    {
        manaCooldownImage.fillAmount = 0f;

        while (manaCooldownImage.fillAmount < 1f)
        {
            manaCooldownImage.fillAmount = Mathf.MoveTowards( 
                manaCooldownImage.fillAmount, 
                1f, (1f / manaCooldown) * Time.deltaTime);

            yield return null;
        }

        manaCooldownImage.fillAmount = 0f;
        onCooldownComplete.Invoke();
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString().PadLeft(3, '0');
    }
}
