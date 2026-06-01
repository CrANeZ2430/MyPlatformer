using System.Collections;
using TMPro;
using UnityEngine;

public class LevelEndSequence : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private Animator flagAnimator;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private TMP_Text coinsText;

    private bool isSequenceTriggered = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(playerTag) && !isSequenceTriggered)
        {
            isSequenceTriggered = true;
            StartCoroutine(RunLevelCompleteSequence());
        }
    }

    private IEnumerator RunLevelCompleteSequence()
    {
        flagAnimator.SetTrigger("PlayerArrived");

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0f;
        levelCompleteUI.SetActive(true);

        var coins = int.Parse(coinsText.text);

        PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, coins);
        PlayerPrefs.Save();
    }
}