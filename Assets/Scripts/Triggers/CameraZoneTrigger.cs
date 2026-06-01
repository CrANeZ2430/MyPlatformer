using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CameraZoneTrigger : MonoBehaviour
{
    [Header("Mode Configuration")]
    [SerializeField] private CameraMode targetMode;
    [SerializeField] private string playerTag = "Player";
    
    [Tooltip("If checked, the camera cuts to this mode instantly without a smooth slide transition.")]
    [SerializeField] private bool instantAnimation = false;

    [Header("Custom Mode Only")]
    [SerializeField] private GameObject customCameraForThisZone;

    [Header("Level Fit Only")]
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [Header("Other Configurations")]
    [SerializeField] float blendTime = 1f;

    private GameObject anchorMin;
    private GameObject anchorMax;

    private void Awake()
    {
        anchorMin = new GameObject($"[{gameObject.name}]_Min");
        anchorMax = new GameObject($"[{gameObject.name}]_Max");
        anchorMin.transform.SetParent(transform);
        anchorMax.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SwitchCameraMode(
                    targetMode, 
                    customCameraForThisZone, 
                    targetTilemap, 
                    targetGroup, 
                    anchorMin, 
                    anchorMax,
                    instantAnimation,
                    blendTime
                );
            }
            else
            {
                Debug.LogError("CameraManager is missing from the scene!");
            }
        }
    }
}