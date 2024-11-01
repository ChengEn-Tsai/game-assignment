using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentControl : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent;
    private Tweener tweener;
    public float pacStudentSpeed;
    private Animator animator;

    public float detectionDistance = 1f;

    public LayerMask wallLayer;

    public Tilemap palletTileMap;
    public TileBase normalPallet;
    public TileBase powerPallet;
    public TileBase emptyTile;

    public ParticleSystem dustParticle;

    [SerializeField] private AudioSource walkingAudioSource;
    private float walkingSoundPeriod => 1 / pacStudentSpeed;
    private float walkingSoundTime = 0;

    [SerializeField] private AudioSource eatingAudioSource;
    private float eatingSoundPeriod => 1 / pacStudentSpeed;
    private float eatingSoundTime = 0;

    private KeyCode lastInput;
    private KeyCode currentInput;

    private bool isWalking => lastInput != KeyCode.None && tweener != null && tweener.tweenerExist();

    private Vector2 currentDirection;
    //private Vector2 nextDirection;
    private string currentStatus = PacStudentStatus.isStill;

    // Start is called before the first frame update
    void Start()
    {
        animator = pacStudent.GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        //nextDirection = currentDirection;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        PacStudentMove();
        dealWithWalkingEffects();

        eatPallet();
    }

    private void PlayerInput()
    {
        Transform transform = pacStudent.transform;
        Vector3 currentPositioin = transform.position;
        if (Input.GetKeyDown(KeyCode.W))
        {
            setInput(KeyCode.W);
            //setDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            setInput(KeyCode.A);
            //setDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            setInput(KeyCode.S);
            //setDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            setInput(KeyCode.D);
            //setDirection(Vector2.right);
        }
    }

    //void setDirection (Vector2 direction)
    //{
    //    nextDirection = direction;
    //    if (!IsWallInDirection(nextDirection))
    //    {
    //        currentDirection = nextDirection;
    //    }
    //}

    void setInput(KeyCode input)
    {
        lastInput = input;
        //if (!IsWall(input))
        //{
        //    currentInput = lastInput;
        //}
    }

    Vector2 getDirectionByKeyCode(KeyCode keyCode)
    {
        if (keyCode == KeyCode.A) { return Vector2.left; }
        else if (keyCode == KeyCode.S) { return Vector2.down; }
        else if (keyCode == KeyCode.D) { return Vector2.right; }
        else if (keyCode == KeyCode.W) { return Vector2.up; }

        return Vector2.zero; // default
    }

    bool IsWall(KeyCode keyCode)
    {
        return IsWallInDirection(getDirectionByKeyCode(keyCode));
    }

    bool IsWallInDirection(Vector2 direction)
    {
        Transform transform = pacStudent.transform;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), direction, detectionDistance, wallLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), direction, detectionDistance, wallLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), direction, detectionDistance, wallLayer);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), direction, detectionDistance, wallLayer);

        return hit1.collider != null || hit2.collider != null || hit3.collider != null || hit4.collider != null;
    }

    //void OnDrawGizmos()
    //{
    //    Transform transform = pacStudent.transform;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, currentDirection * detectionDistance);
    //}

    void moveToward(KeyCode input)
    {
        if (tweener != null && !tweener.tweenerExist())
        {
            currentDirection = getDirectionByKeyCode(input);
            Transform transform = pacStudent.transform;
            Vector2 currentPositioin = transform.position;
            Vector2 nextTerminal = currentPositioin + currentDirection;

            float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;
            string newStatus = getStatus(currentDirection);

            tweener.AddTween(
                transform,
                currentPositioin,
                nextTerminal,
                duration
            );
            setAnimationStatus(newStatus);
        }
    }
    private void PacStudentMove()
    {
        if (!IsWall(lastInput))
        {
            currentInput = lastInput;
            moveToward(lastInput);
        }
        else if (!IsWall(currentInput))
        {
            moveToward(currentInput);
        }
        //if (!IsWallInDirection(nextDirection) && nextDirection != currentDirection)
        //{
        //    currentDirection = nextDirection;
        //    setAnimationStatus(getStatus(currentDirection));
        //}
        //else if (!IsWallInDirection(currentDirection))
        //{
        //    if (tweener != null && !tweener.tweenerExist())
        //    {
        //        Transform transform = pacStudent.transform;
        //        Vector2 currentPositioin = transform.position;
        //        Vector2 nextTerminal = currentPositioin + currentDirection;

        //        Debug.Log("New! " + currentPositioin);
        //        float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;
        //        string newStatus = getStatus(currentDirection);

        //        tweener.AddTween(
        //            transform,
        //            currentPositioin,
        //            nextTerminal,
        //            duration
        //        );

        //        if (currentStatus != newStatus)
        //        {
        //            setAnimationStatus(newStatus);
        //        }
        //    }
        //}
        else
        {
            setAnimationStatus(PacStudentStatus.isStill);
        }
    }

    void setAnimationStatus(string status)
    {
        string[] allStatus = new string[6] { PacStudentStatus.WalkingUp, PacStudentStatus.WalkingDown, PacStudentStatus.WalkingLeft, PacStudentStatus.WalkingRight, PacStudentStatus.isDead, PacStudentStatus.isStill };

        for (int i = 0; i < allStatus.Length; i++)
        {
            if (status == allStatus[i])
            {
                animator.SetBool(allStatus[i], true);
            }
            else
            {
                animator.SetBool(allStatus[i], false);
            }
        }
    }

    private string getStatus(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return PacStudentStatus.WalkingUp;
        }
        else if (direction == Vector2.down)
        {
            return PacStudentStatus.WalkingDown;
        }
        else if (direction == Vector2.left)
        {
            return PacStudentStatus.WalkingLeft;
        }
        else if (direction == Vector2.right)
        {
            return PacStudentStatus.WalkingRight;
        }
        else
        {
            return PacStudentStatus.isStill;
        }
    }

    void dealWithWalkingEffects()
    {
        if (isWalking)
        {
            if (eatingAudioSource.isPlaying)
            {
            }
            else if (!walkingAudioSource.isPlaying || Time.time - walkingSoundTime >= walkingSoundPeriod)
            {
                walkingSoundTime = Time.time;
                walkingAudioSource.Play();
                DustPlay();
            }
        }
        else
        {
            walkingAudioSource.Stop();
            dustParticle.Stop();
        }

    }

    void DustPlay()
    {
        Transform transform = dustParticle.transform;
        if (currentDirection == Vector2.right)
        {
            transform.localPosition = new Vector2(-0.5f, -0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (currentDirection == Vector2.up)
        {
            transform.localPosition = new Vector2(0, -0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (currentDirection == Vector2.left)
        {
            transform.localPosition = new Vector2(0.5f, -0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (currentDirection == Vector2.down)
        {
            transform.localPosition = new Vector2(0, 0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        dustParticle.Play();
    }

    void eatPallet()
    {
        Vector3Int tilePosition = palletTileMap.WorldToCell(pacStudent.transform.position);
        TileBase tile = palletTileMap.GetTile(tilePosition);

        if (tile == normalPallet)
        {
            eatingSoundTime = Time.time;
            palletTileMap.SetTile(tilePosition, null);
            walkingAudioSource.Stop();
            eatingAudioSource.Play();
            DustPlay();
        }
        else if (tile == powerPallet)
        {
            eatingSoundTime = Time.time;
            palletTileMap.SetTile(tilePosition, emptyTile);

            DustPlay();
        }
        else
        {
            if (Time.time - eatingSoundTime >= eatingSoundPeriod)
            {
                eatingAudioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with a object!" + other.name);
        // Check if the object has the "Destroyable" tag
        if (other.CompareTag("PowerPellet"))
        {
            Debug.Log("Get PowerPellet!!!");
            Destroy(other.gameObject);  // Destroy the object
        }
        if (other.CompareTag("BonusCherry"))
        {
            Debug.Log("Catch Bonus Cherry~~~");
            Destroy(other.gameObject);  // Destroy the object
        }
    }


    public class PacStudentStatus
    {
        public static string WalkingUp = "WalkingUp";
        public static string WalkingDown = "WalkingDown";
        public static string WalkingLeft = "WalkingLeft";
        public static string WalkingRight = "WalkingRight";
        public static string isDead = "isDead";
        public static string isStill = "isStill";
    }
}
