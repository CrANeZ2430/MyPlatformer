using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image healthBar, manaBar;

    public void ChangeHealthBar(int currentHealth)
    {
        healthBar.fillAmount = currentHealth / 100f;
    }

    public void ChangeManaBar(int currentMana)
    {
        manaBar.fillAmount = currentMana / 100f;

    }
}
