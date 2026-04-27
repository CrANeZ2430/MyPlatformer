using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";
    [SerializeField] Animator playerAnimator;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            playerAnimator.SetBool(IS_RUNNING, false);
        }
        else
        {
            playerAnimator.SetBool(IS_RUNNING, true);
        }
    }
}
