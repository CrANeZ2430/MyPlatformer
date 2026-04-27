using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private bool menuOpen;

    public bool GetMenuOpen()
    {
        return menuOpen;
    }

    public void ShowPanel()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        menuOpen = menuPanel.activeSelf;
        Time.timeScale = menuPanel.activeSelf ? 0 : 1;
    }
}
