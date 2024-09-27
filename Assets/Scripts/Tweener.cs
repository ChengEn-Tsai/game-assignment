using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTween != null)
        {
            float distance = Vector3.Distance(activeTween.Target.transform.position, activeTween.EndPos);
            
            if (distance > 0.1f && Time.time <= activeTween.StartTime + activeTween.Duration)
            {
                float t = (Time.time - activeTween.StartTime) / activeTween.Duration;

                Vector3 nextPosition =
                    Vector3.Lerp(
                        activeTween.StartPos,
                        activeTween.EndPos,
                        t
                    );
                //Debug.Log("nextPosition: " + nextPosition);
                activeTween.Target.position = nextPosition;
            }
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
            }
        }
    }

    public void AddTween(
        Transform targetObject,
        Vector3 startPos,
        Vector3 endPos,
        float duration)
    {
        activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
    }

    public bool tweenerExist ()
    {
        return activeTween != null;
    }
}
