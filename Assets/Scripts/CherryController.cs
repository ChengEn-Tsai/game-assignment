using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CherryController : MonoBehaviour
{
    [SerializeField]
    private GameObject CherryPrefab;

    //private Tweener tweener;

    float minY = 16.0f;
    float minX = 27.0f;
    // Start is called before the first frame update
    void Start()
    {
        //tweener = GetComponent<Tweener>();
        InvokeRepeating("CreateCherry", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CreateCherry()
    {
        GameObject cherry;
        Transform cherryTransform;

        cherry = Instantiate(CherryPrefab);
        cherry.tag = "BonusCherry";
        cherryTransform = cherry.transform;
        Vector2 position = getARandomPosition();
        cherryTransform.position = position;
        Tweener tweener = moveCherry(cherryTransform);

        StartCoroutine(DestroyCherryAfterDelay(cherry, tweener, 20f));
    }

    Tweener moveCherry(Transform cherryTransform)
    {
        Tweener tweener = gameObject.AddComponent<Tweener>();
        Vector2 currentPosition = cherryTransform.position;
        Vector2 nextPosition = new Vector2(-currentPosition.x, -currentPosition.y);
        float distance = Vector2.Distance(nextPosition, currentPosition);
        tweener.AddTween(
            cherryTransform,
            currentPosition,
            nextPosition,
            0.25f * distance
        );
        return tweener;
    }

    Vector2 getARandomPosition ()
    {
        float posX, posY;
        if (Random.value > 0.5f) {
            float randomSign = Random.value > 0.5f ? 1.0f : -1.0f;
            posX = randomSign * minX;
            posY = Random.Range(-minY, minY);
        } else
        {
            float randomSign = Random.value > 0.5f ? 1.0f : -1.0f;
            posY = randomSign * minY;
            posX = Random.Range(-minX, minX);
        }

        return new Vector2(posX, posY);
    }

    IEnumerator DestroyCherryAfterDelay(GameObject cherry, Tweener tweener, float delay)
    {
        yield return new WaitForSeconds(delay);
        destroyCherry(cherry, tweener);
        //destroyCherry(cherry);
    }

    private void destroyCherry(GameObject cherry, Tweener tweener)
    {
        Destroy(tweener);
        Destroy(cherry);
    }

    //IEnumerator DestroyTweenerAfterDelay(Component tweener, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    destroyCherry(tweener);
    //    //destroyCherry(cherry);
    //}

    //private void destroyCherry(GameObject cherry)
    //{
    //    Destroy(cherry);
    //}
}
