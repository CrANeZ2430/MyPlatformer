using UnityEngine;

public enum CameraMode { Deadzone, LevelFit, Custom }

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Global Default Cameras")]
    [SerializeField] private GameObject deadzoneCam;
    [SerializeField] private GameObject levelFitCam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwitchCameraMode(CameraMode mode, GameObject specificCustomCam, UnityEngine.Tilemaps.Tilemap tilemap, Cinemachine.CinemachineTargetGroup targetGroup, GameObject anchorMin, GameObject anchorMax, bool instant)
    {
        if (instant)
        {
            // This force-bypasses the active transition for the next frame swap
            #if UNITY_6000_0_OR_NEWER
            var brain = Object.FindFirstObjectByType<Cinemachine.CinemachineBrain>();
            if (brain != null)
            {
                // This forces an immediate cut, bypassing transition curves entirely
                brain.m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;
                
                // We will restore it a split second later using a hidden invoke loop
                Invoke(nameof(RestoreNormalBlendStyle), 0.05f);
            }
            #endif
        }

        // 2. Reset all priorities back to baseline
        SetPriority(deadzoneCam, 10);
        SetPriority(levelFitCam, 10);

        var allVCams = Object.FindObjectsByType<Cinemachine.CinemachineVirtualCamera>(FindObjectsSortMode.None);
        foreach (var vcam in allVCams)
        {
            if (vcam.gameObject != deadzoneCam && vcam.gameObject != levelFitCam)
            {
                vcam.Priority = 10;
            }
        }

        // 3. Identify our target camera to elevate
        GameObject targetCamObj = null;

        switch (mode)
        {
            case CameraMode.Deadzone:
                targetCamObj = deadzoneCam;
                break;

            case CameraMode.LevelFit:
                if (tilemap != null && targetGroup != null)
                {
                    UpdateBounds(tilemap, targetGroup, anchorMin, anchorMax);
                }
                targetCamObj = levelFitCam;
                break;

            case CameraMode.Custom:
                targetCamObj = specificCustomCam;
                break;
        }

        // 4. Fire the priority boost
        if (targetCamObj != null)
        {
            var vcam = targetCamObj.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Priority = 20;
            }
        }
    }

    private void RestoreNormalBlendStyle()
    {
        var brain = Object.FindFirstObjectByType<Cinemachine.CinemachineBrain>();
        if (brain != null)
        {
            // Restore your normal default smooth animation style (EaseInOut)
            brain.m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.EaseInOut;
        }
    }

    private void SetPriority(GameObject camObj, int priority)
    {
        if (camObj != null)
        {
            var vcam = camObj.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            if (vcam != null) vcam.Priority = priority;
        }
    }

    private void UpdateBounds(UnityEngine.Tilemaps.Tilemap tilemap, Cinemachine.CinemachineTargetGroup targetGroup, GameObject min, GameObject max)
    {
        Bounds bounds = tilemap.localBounds;
        min.transform.position = tilemap.transform.TransformPoint(bounds.min);
        max.transform.position = tilemap.transform.TransformPoint(bounds.max);

        if (targetGroup.m_Targets != null)
        {
            for (int i = targetGroup.m_Targets.Length - 1; i >= 0; i--)
            {
                if (targetGroup.m_Targets[i].target != null)
                    targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
            }
        }
        targetGroup.AddMember(min.transform, 1f, 1f);
        targetGroup.AddMember(max.transform, 1f, 1f);
    }
}