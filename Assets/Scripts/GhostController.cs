using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GhostController : MonoBehaviour
{
    public int normalStatusDirectionMode;
    [SerializeField] private Transform pacStudentTransform;
    [SerializeField] private float ghostSpeed;
    private Vector2[] directions =
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };
    ScaryModeController scaryModeController;
    private Animator animator;
    public bool isDead = false;

    public float detectionDistance = 1.1f;

    public LayerMask wallLayer;
    public LayerMask doorLayer;

    [SerializeField] private AudioSource ghostDeathAudioSource;

    private bool hasTouchedCornerPoint = false;
    private Vector2 CornerPoint = new Vector2(-12.5f, 12.5f);

    private Tweener tweener;

    private Vector2 currentDirection;

    private GameObject transportLeft;
    private GameObject transportRight;

    private Vector2 rebornPoint = new Vector2(0.5f, 0.5f);
    private bool isOutside = false;
    private Vector2 startPoint = new Vector2(0.5f, 2.5f);

    // Start is called before the first frame update
    void Start()
    {
        GameObject Manager = GameObject.Find("Manager");
        scaryModeController = Manager.GetComponent<ScaryModeController>();
        animator = gameObject.GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        transportLeft = GameObject.Find("TransporterLeft");
        transportRight = GameObject.Find("TransporterRight");
    }

    // Update is called once per frame
    void Update()
    {
        ControlGhostStatus();

        move();
    }

    void ControlGhostStatus()
    {
        if (scaryModeController.GetScaryMode())
        {
            animator.SetBool("IsScared", true);
        } else
        {
            animator.SetBool("IsScared", false);
        }

        if (scaryModeController.GetRecoveringMode())
        {
            animator.SetBool("IsRecovering", true);
        }
        else
        {
            animator.SetBool("IsRecovering", false);
        }
    }

    public void GhostDies ()
    {
        ghostDeathAudioSource.Play();
        isDead = true;
        animator.SetBool("IsDead", true);

        tweener.stop();
        tweener.AddTween(
            transform,
            transform.position,
            rebornPoint,
            0.1f * (float)Vector2.Distance(transform.position, rebornPoint)
        );
        Invoke("GhostComesBack", 7);
    }

    void GhostComesBack()
    {
        isDead = false;
        isOutside = false;
        animator.SetBool("IsDead", false);
    }

    bool IsWallInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, wallLayer);
        bool hitTheDoor = false;
        
        if (isOutside) 
        {
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, direction, detectionDistance, doorLayer);
            if (hit2.collider != null)
            {
                hitTheDoor = true;
            }
        }

        return (hit.collider != null) || hitTheDoor;
    }

    //bool IsWallInDirection(Vector2 direction)
    //{
    //    RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0.4f, 0, 0), direction, detectionDistance, wallLayer);
    //    RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(-0.4f, 0, 0), direction, detectionDistance, wallLayer);
    //    RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(0, 0.4f, 0), direction, detectionDistance, wallLayer);
    //    RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(0, -0.4f, 0), direction, detectionDistance, wallLayer);

    //    return hit1.collider != null || hit2.collider != null || hit3.collider != null || hit4.collider != null;
    //}

    public Vector2 GetNextDirection()
    {
        if (!isOutside)
        {
            return useSecondModeToGetNextDirection(startPoint);
        }
        if (scaryModeController.GetScaryMode())
        {
            hasTouchedCornerPoint = false;
            return useFirstModeToGetNextDirection();
        }
        else
        {
            switch (normalStatusDirectionMode)
            {
                case 1:
                    return useFirstModeToGetNextDirection();
                case 2:
                default:
                    return useSecondModeToGetNextDirection(pacStudentTransform.position);
                case 3:
                    return useThirdModeToGetNextDirection();
                case 4:
                    return useFourthModeToGetNextDirection();
            }
        }
    }

    private Vector2 useFirstModeToGetNextDirection()
    {
        Vector2 pacStudentPosition = pacStudentTransform.position;
        Vector2 ghostPosition = transform.position;
        Vector2 pacStudentRelativePosition = pacStudentPosition - ghostPosition;
        Vector2 farestDirection = Vector2.zero;
        float farestDistance = 0;
        foreach (Vector2 Direction in directions)
        {
            if (Direction != (-1 * currentDirection) && !IsWallInDirection(Direction))
            {
                float distance = Vector2.Distance(pacStudentRelativePosition, Direction);
                if (distance > farestDistance)
                {
                    farestDistance = distance;
                    farestDirection = Direction;
                }
            }
        }
        return farestDirection;
    }

    private Vector2 useSecondModeToGetNextDirection(Vector2 targetPosition)
    {
        Vector2 ghostPosition = transform.position;
        Vector2 pacStudentRelativePosition = targetPosition - ghostPosition;
        Vector2 closestDirection = Vector2.zero;
        float closestDistance = 9999.0f;
        foreach (Vector2 Direction in directions)
        {
            if (Direction != (-1 * currentDirection) && !IsWallInDirection(Direction))
            {
                float distance = Vector2.Distance(pacStudentRelativePosition, Direction);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDirection = Direction;
                }
            }
        }
        return closestDirection;
    }

    private Vector2 useThirdModeToGetNextDirection()
    {
        Vector2 randomDirection;
        while(true)
        {
            int randomIndex = Random.Range(0, 4);
            Vector2 Direction = directions[randomIndex];
            if (Direction != (-1 * currentDirection) && !IsWallInDirection(Direction))
            {
                randomDirection = Direction;
                break;
            }
        }
        return randomDirection;
    }

    private Vector2 useFourthModeToGetNextDirection()
    {
        if (hasTouchedCornerPoint)
        {
            float x = currentDirection.x;
            float y = currentDirection.y;
            Vector2[] directions =
            {
                new Vector2(-y, x), currentDirection, new Vector2(y, -x)
            };
            foreach (Vector2 direction in directions)
            {
                if (!IsWallInDirection(direction))
                {
                    return direction;
                }
            }
            return Vector2.zero;
        } 
        else
        {
            Vector2 ghostPosition = transform.position;
            if (Vector2.Distance(CornerPoint, ghostPosition) == 0)
            {
                hasTouchedCornerPoint = true;
                return useFourthModeToGetNextDirection();
            } 
            else
            {
                return useSecondModeToGetNextDirection(CornerPoint);
            }
        }
    }

    void move()
    {
        if (tweener != null && !tweener.tweenerExist())
        {
            currentDirection = GetNextDirection();
            
            Vector2 currentPositioin = transform.position;
            Vector2 nextTerminal = currentPositioin + (currentDirection);
            float duration = Vector3.Distance(currentPositioin, nextTerminal) / ghostSpeed;
            tweener.AddTween(
                transform,
                currentPositioin,
                nextTerminal,
                duration
            );
            setAnimation(currentDirection);
        }
    }

    private string getStatus(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return GhostStatus.WalkingUp;
        }
        else if (direction == Vector2.down)
        {
            return GhostStatus.WalkingDown;
        }
        else if (direction == Vector2.left)
        {
            return GhostStatus.WalkingLeft;
        }
        else if (direction == Vector2.right)
        {
            return GhostStatus.WalkingRight;
        }
        else
        {
            return GhostStatus.IsScared;
        }
    }

    void setAnimation (Vector2 direction)
    {
        foreach (Vector2 dir in directions)
        {
            string status = getStatus(dir);
            if (direction == dir)
            {
                animator.SetBool(status, true);
            } else
            {
                animator.SetBool(status, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TransporterRight"))
        {
            Vector2 transportLeftPos = transportLeft.transform.position;
            transform.position = transportLeftPos + new Vector2(2f, 0);
            tweener.stop();
        }
        if (other.CompareTag("TransporterLeft"))
        {
            Vector2 transportRightPos = transportRight.transform.position;
            transform.position = transportRightPos - new Vector2(2f, 0);
            tweener.stop();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("door"))
        {
            isOutside = true;
        }
    }

    public class GhostStatus
    {
        public static string WalkingUp = "WalkingUp";
        public static string WalkingDown = "WalkingDown";
        public static string WalkingLeft = "WalkingLeft";
        public static string WalkingRight = "WalkingRight";
        public static string IsDead = "IsDead";
        public static string IsScared = "IsScared";
        public static string IsRecovering = "IsRecovering";
    }
}
