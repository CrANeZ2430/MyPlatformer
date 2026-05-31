using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenuPanel;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle soundToggle;
    
    private bool isPaused = false;
    private const string MIXER_PARAMETER = "MasterVol";
    private const string PREFS_KEY = "SoundMuted";

    void Start()
    {
        if (soundToggle != null)
        {
            bool isMuted = PlayerPrefs.GetInt(PREFS_KEY, 0) == 1;
            soundToggle.isOn = !isMuted;
            SetVolume(!isMuted);

            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }
    }

    void Update()
    {
        // Toggle pause when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Pause logic
    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    // Audio logic
    private void OnSoundToggleChanged(bool isSoundOn)
    {
        SetVolume(isSoundOn);
        PlayerPrefs.SetInt(PREFS_KEY, isSoundOn ? 0 : 1);
        PlayerPrefs.Save();
    }

    private void SetVolume(bool isSoundOn)
    {
        if (audioMixer == null) return;
        float targetVolume = isSoundOn ? 0f : -80f;
        audioMixer.SetFloat(MIXER_PARAMETER, targetVolume);
    }

    private void OnDestroy()
    {
        // Clean up the listener when this object is destroyed
        if (soundToggle != null)
            soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        
        SceneManager.LoadScene("MainMenu");
    }
}