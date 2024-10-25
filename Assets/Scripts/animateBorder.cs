using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateBorder : MonoBehaviour
{
    [SerializeField]
    public List<Transform> TurnPoints;

    public float speed;

    public int pelletCount;

    public GameObject PelletImagePrefab;
    public GameObject canvas;

    private List<GameObject> pellets;
    private List<float> pelletPos;
    private List<float> turnPointDistanceFromFirstOne = new List<float>();

    private float totalDistance;
    // Start is called before the first frame update
    void Start()
    {
        totalDistance = 0;
        turnPointDistanceFromFirstOne.Add(0);
        for (int i = 1; i < TurnPoints.Count; i++)
        {
            totalDistance += Vector3.Distance(TurnPoints[i].position, TurnPoints[i - 1].position);
            turnPointDistanceFromFirstOne.Add(totalDistance);
        }
        totalDistance += Vector3.Distance(TurnPoints[TurnPoints.Count - 1].position, TurnPoints[0].position);

        pellets = new List<GameObject>();
        pelletPos = new List<float>();

        for (int i = 0; i < pelletCount; i++)
        {
            pelletPos.Add((totalDistance / pelletCount) * i);
            pellets.Add(Instantiate(PelletImagePrefab));
            pellets[i].transform.SetParent(gameObject.transform, false);
            pellets[i].transform.position = setPosition(pelletPos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pellets.Count; i++)
        {
            pelletPos[i] += speed;
            pelletPos[i] %= totalDistance;
            pellets[i].transform.position = setPosition(pelletPos[i]);
        }
    }

    Vector3 setPosition(float x)
    {
        int segment = 0, nextSegment = 0;
        //Find Segment
        for (int i = 0; i < TurnPoints.Count; i++)
        {
            if (x >= turnPointDistanceFromFirstOne[i])
            {
                segment = i;
            }
            else
            {
                nextSegment = i;
                break;
            }
        }



        float percentProgress = (x - turnPointDistanceFromFirstOne[segment]) / Vector3.Distance(TurnPoints[segment].position, TurnPoints[nextSegment].position);
        return Vector3.Lerp(TurnPoints[segment].position, TurnPoints[nextSegment].position, percentProgress);
    }
}
