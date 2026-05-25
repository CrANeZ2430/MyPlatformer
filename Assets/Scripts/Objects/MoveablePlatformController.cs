using UnityEngine;

public class MoveablePlatformController : MonoBehaviour
{
    [SerializeField] private Vector3 startPos, endPos;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    [SerializeField] private float speed;
    [SerializeField] private float platformSize;
    [SerializeField] private bool isMovingHorizontaly;

    private Vector3 targetPos;
    private PlayerController playerController;

    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Start()
    {
        ChangePositionSettings();
    }

    void Update()
    {
        PlatformMovement();
    }

    private void PlatformMovement()
    {
        if (Vector3.Distance(startPos, transform.position) < 0.05f)
        {
            targetPos = endPos;
        }
        else if (Vector3.Distance(transform.position, endPos) < 0.05f)
        {
            targetPos = startPos;
        }

        if (!isMovingHorizontaly)
        {
            if (CheckPlatformObstacle(groundLayer))
            {
                targetPos = startPos;
            }
            else if (CheckPlatformObstacle(playerLayer) && playerController.IsGrounded)
            {
                targetPos = startPos;
                playerController.Damage();
            }
        }


        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
    
    private void ChangePositionSettings()
    {
        if (isMovingHorizontaly)
        {
            startPos.y = transform.position.y;
            endPos.y = transform.position.y;
        }
        else
        {
            startPos.x = transform.position.x;
            endPos.x = transform.position.x;
        }

        platformSize -= 0.1f;
        targetPos = endPos;
    }

    private bool CheckPlatformObstacle(LayerMask checkLayer)
    {
        var boxAngle = 0f;
        var boxDist = 0.05f;
        var boxOrigin = transform.position;
        var boxSize = new Vector2(platformSize, boxDist);
        var boxDir = Vector2.down;

        var platformReturn = Physics2D.BoxCast(boxOrigin, boxSize, boxAngle, 
                                                boxDir, boxDist, checkLayer) && targetPos != startPos;

        return platformReturn;
    }
}
