using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelSelector : MonoBehaviour
{
    [SerializeField] private string[] levels;
    [SerializeField] private TMP_Text[] coinTexts;
    [SerializeField] private Button[] levelButtons;


    void Start()
    {
        UpdatePrefs();
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UpdatePrefs();
    }

    private void UpdatePrefs()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            var buttonImage = levelButtons[i].GetComponent<Image>();

            if (PlayerPrefs.HasKey(levels[i]))
            {
                coinTexts[i].text = PlayerPrefs.GetInt(levels[i]).ToString().PadLeft(3, '0');
                if (buttonImage != null) buttonImage.color = Color.green;
            }
            else
            {
                coinTexts[i].text = "000";
                if (buttonImage != null) buttonImage.color = Color.white;
            }
        }
    }
}
