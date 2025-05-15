using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPole : MonoBehaviour
{
    //public GameObject pointPrefab;
    public Transform flag;
    public Transform bottom;
    public float flagVelocity = 3f;

    bool downFlag;
    Move mover;

    public GameObject pointPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Move>();
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            //Instantiate(pointPrefab, contactPoint, Quaternion.identity);
            CalculateHeight(contactPoint.y);
        }
    }

    private void FixedUpdate()
    {
        if (downFlag)
        {
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - flagVelocity * Time.fixedDeltaTime);
            }
            else
            {
                mover.isFlagdown = true;
            }
        }
    }

    void CalculateHeight(float marioPosition)
    {
        float size = GetComponent<BoxCollider2D>().bounds.size.y;
        //Debug.Log("Totalsize: " + size);

        float minPos1 = transform.position.y + (size - size / 5f); // Parte más alta = 5000 pts
        //Debug.Log("Min1: " + minPos1);

        float minPos2 = transform.position.y + (size - 2 * size / 5f); // Parte más alta = 2000 pts
        //Debug.Log("Min2: " + minPos2);

        float minPos3 = transform.position.y + (size - 3 * size / 5f); // Parte más alta = 1000 pts
        //Debug.Log("Min3: " + minPos3);

        float minPos4 = transform.position.y + (size - 4 * size / 5f); // Parte más alta = 400 pts
                                                                       //Debug.Log("Min4: " + minPos4);

        int numPoints = 0;
        if (marioPosition >= minPos1)
        {
            numPoints = 5000;
        }
        else if (marioPosition >= minPos2)
        {
            numPoints = 2000;
        }
        else if (marioPosition >= minPos3)
        {
            numPoints = 800;
        }
        else if (marioPosition >= minPos4)
        {
            numPoints = 400;
        }
        else
        {
            numPoints = 200;
        }
        ScoreManager.instance.AddScore(numPoints);

        Vector2 positionPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoint = Instantiate(pointPrefab, positionPoints, Quaternion.identity);
        Points points = newFloatPoint.GetComponent<Points>();
        points.numPoints = numPoints;
        points.speed = flagVelocity;

        points.distance = flag.position.y - bottom.position.y;
        points.destroy = false;
    }
}
