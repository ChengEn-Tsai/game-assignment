using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent;
    private Tweener tweener;
    public float pacStudentSpeed = 2.0f;
    private Animator animator;

    Vector3 topLeftPos = new Vector3(-12.5f, 12.5f, 0);
    Vector3 topRightPos = new Vector3(-7.5f, 12.5f, 0);
    Vector3 bottomLeftPos = new Vector3(-12.5f, 8.5f, 0);
    Vector3 bottomRightPos = new Vector3(-7.5f, 8.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 2.5f;

        animator = pacStudent.GetComponent<Animator>();
        //animator.SetBool("WalkingRight", true);
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {

        Transform transform = pacStudent.transform;
        Vector3 currentPositioin = transform.position;

        if (currentPositioin == topLeftPos)
        {
            Vector3 nextTerminal = topRightPos;
            float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;

            if (tweener != null && !tweener.tweenerExist())
            {
                setAnimationStatus("WalkingRight");
                tweener.AddTween(
                    transform,
                    currentPositioin,
                    nextTerminal,
                    duration
                );
            }

        }
        else if (currentPositioin == topRightPos)
        {
            Vector3 nextTerminal = bottomRightPos;
            float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;

            if (tweener != null && !tweener.tweenerExist())
            {
                setAnimationStatus("WalkingDown");
                tweener.AddTween(
                    transform,
                    currentPositioin,
                    nextTerminal,
                    duration
                );
            }

        }
        else if (currentPositioin == bottomRightPos)
        {
            Vector3 nextTerminal = bottomLeftPos;
            float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;

            if (tweener != null && !tweener.tweenerExist())
            {
                setAnimationStatus("WalkingLeft");
                tweener.AddTween(
                    transform,
                    currentPositioin,
                    nextTerminal,
                    duration
                );
            }

        }
        else if (currentPositioin == bottomLeftPos)
        {
            Vector3 nextTerminal = topLeftPos;
            float duration = Vector3.Distance(currentPositioin, nextTerminal) / pacStudentSpeed;

            if (tweener != null && !tweener.tweenerExist())
            {
                setAnimationStatus("WalkingUp");
                tweener.AddTween(
                    transform,
                    currentPositioin,
                    nextTerminal,
                    duration
                );
            }

        }
    }

    void setAnimationStatus(string status)
    {
        string[] allStatus = new string[5] { "WalkingUp", "WalkingDown", "WalkingLeft", "WalkingRight", "isDead" };

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
}
