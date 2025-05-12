using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPole : MonoBehaviour
{
    public Transform flag;
    public Transform bottom;
    public float flagVelocity = 3f;

    bool downFlag;
    Move mover;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Move>();
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
}
