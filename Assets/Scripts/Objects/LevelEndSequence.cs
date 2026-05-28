using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndSequence : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string sceneToLoad = "MainMenu";

    [Header("References")]
    [SerializeField] private Animator flagAnimator; // Drag the flag fabric Animator here
    [SerializeField] private GameObject levelCompleteUI; // Drag the Canvas Panel here

    private bool isSequenceTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isSequenceTriggered)
        {
            isSequenceTriggered = true;
            StartCoroutine(RunLevelCompleteSequence());
        }
    }

    private IEnumerator RunLevelCompleteSequence()
    {
        if (flagAnimator != null)
        {
            flagAnimator.SetTrigger("PlayerArrived");
        }

        yield return new WaitForSeconds(1.5f);

        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }
    }
}