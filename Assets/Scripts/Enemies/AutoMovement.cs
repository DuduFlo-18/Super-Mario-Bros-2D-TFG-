using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{

    public float speed = 1f;
    bool movementPaused;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    Vector2 lastVelocity;
    Vector2 currentDirection;
    float defaultSpeed;

    public bool flipSprite = true;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        defaultSpeed = Mathf.Abs(speed);
    }

    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            if (rb2d.velocity.x> -0.1f && rb2d.velocity.x < 0.1)
        {
            speed = -speed;
        }
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);

        if(flipSprite)
            {
                if(rb2d.velocity.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        }
    } 

    public void PauseMovement()
    {
        if(!movementPaused) 
        {
            currentDirection = rb2d.velocity.normalized;
            lastVelocity = rb2d.velocity;
            movementPaused = true;
            rb2d.velocity = new Vector2(0,0);
        }
    }

    public void ContinueMovement()
    {
        if (movementPaused)
        {
            speed = defaultSpeed*currentDirection.x;
            rb2d.velocity = new Vector2(speed, lastVelocity.y);
            movementPaused = false;    
        }
    }

    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            rb2d.velocity = newVelocity;
            movementPaused = false;
        }
    }
}
