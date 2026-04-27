using UnityEngine;

public class MoveablePlatformController : MonoBehaviour
{
    [SerializeField] private Vector3 startPos, endPos;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    [SerializeField] private float speed;
    [SerializeField] private float platformSize;
    [SerializeField] private bool isMovingHorizontaly;

    private Vector3 targetPos;
    private Player playerComponent;
    private UI uiComponent;

    private void Awake()
    {
        playerComponent = FindObjectOfType<Player>();
        uiComponent = FindObjectOfType<UI>();
    }

    private void Start()
    {
        ChangePositionSettings();
    }

    private void Update()
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
            else if (CheckPlatformObstacle(playerLayer) && playerComponent.CheckGround())
            {
                targetPos = startPos;
                uiComponent.ChangeUIValue(-1, 0);
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
        float boxAngle = 0f;
        float boxDist = 0.05f;
        Vector2 boxOrigin = transform.position;
        Vector2 boxSize = new Vector2(platformSize, boxDist);
        Vector2 boxDir = Vector2.down;

        bool platformReturn = Physics2D.BoxCast(boxOrigin, boxSize, boxAngle, 
                                                boxDir, boxDist, checkLayer)&& targetPos != startPos;

        return platformReturn;
    }
}
