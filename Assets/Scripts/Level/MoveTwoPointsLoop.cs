using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTwoPointsLoop : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    Vector3 currentTarget;
    Vector3 startPoint;
    Vector3 endPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = pointA.position;
        endPoint = pointB.position;
        currentTarget = endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);
        if (transform.position == currentTarget)
        {
            if (currentTarget == endPoint)
            {
                currentTarget = startPoint;
            }
            else
            {
                currentTarget = endPoint;
            }
        }
    }
}
