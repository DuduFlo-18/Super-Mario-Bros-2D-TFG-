using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform showPoint;
    public Transform hidePoint;

    public float waitForHide;
    public float waitForShow;

    public float speedShow;
    public float speedHide;


    float timershow;
    float timerhide;

    float speed;
    Vector2 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = hidePoint.position;
        speed = speedHide;
        timerhide = 0;
        timershow = 0;
    }

    // Update is called once per frame
    void Update()
    {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, targetPoint, speed*Time.deltaTime);

        if (Vector2.Distance(objectToMove.transform.position, hidePoint.position) < 0.1f)
        {
            //Si esta escondido
            timershow += Time.deltaTime;
            if (timershow >= waitForShow && !Locked())
            {
                targetPoint = showPoint.position;
                speed = speedShow;
                timershow = 0;
            }
        }

        else if (Vector2.Distance(objectToMove.transform.position, showPoint.position)< 0.1f)
        {
            //Si la planta se esta mostrando
            timerhide += Time.deltaTime;
            if (timerhide >= waitForShow)
            {
                targetPoint = hidePoint.position;
                speed = speedHide;
                timerhide= 0;
            }
        }
    }

    bool Locked()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.up, Vector2.one, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector2.one);
    }
}
