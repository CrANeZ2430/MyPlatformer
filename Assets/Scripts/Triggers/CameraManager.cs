using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public enum CameraMode { Deadzone, LevelFit, Custom }

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Global Default Cameras")]
    [SerializeField] private GameObject deadzoneCam;
    [SerializeField] private GameObject levelFitCam;
    //[SerializeField] private GameObject mainCam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwitchCameraMode(CameraMode mode, GameObject specificCustomCam, Tilemap tilemap, CinemachineTargetGroup targetGroup, GameObject anchorMin, GameObject anchorMax, bool instant, float blendTime = 2.0f)
    {
        // 1. Handle blend styles and custom blend times
        var brain = FindFirstObjectByType<CinemachineBrain>();
        if (brain != null)
        {
            if (instant)
            {
                // Forces an immediate cut, bypassing transition curves entirely
                brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                
                // We will restore it a split second later using a hidden invoke loop
                Invoke(nameof(RestoreNormalBlendStyle), 0.05f);
            }
            else
            {
                // Apply the custom transition style and duration dynamically
                brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                brain.m_DefaultBlend.m_Time = blendTime;
            }
        }

        // 2. Reset all priorities back to baseline
        SetPriority(deadzoneCam, 10);
        SetPriority(levelFitCam, 10);
        //SetPriority(mainCam, 20);

        var allVCams = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);
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
            var vcam = targetCamObj.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Priority = 20;
            }
        }
    }

    private void RestoreNormalBlendStyle()
    {
        var brain = FindFirstObjectByType<CinemachineBrain>();
        if (brain != null)
        {
            // Restore your normal default smooth animation style (EaseInOut)
            brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        }
    }

    private void SetPriority(GameObject camObj, int priority)
    {
        if (camObj != null)
        {
            var vcam = camObj.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null) vcam.Priority = priority;
        }
    }

    private void UpdateBounds(Tilemap tilemap, CinemachineTargetGroup targetGroup, GameObject min, GameObject max)
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