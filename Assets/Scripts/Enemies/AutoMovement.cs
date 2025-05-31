using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    bool hasbeenVisible;

   

    float timer = 0;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        //rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        defaultSpeed = Mathf.Abs(speed);
        rb2d.isKinematic = true;
        movementPaused = true;
    }

    private void Update()
    {
       if (spriteRenderer.isVisible && !hasbeenVisible)
       {
            ActivateMovement();
       }
    }

    // Activa el movimiento automático del enemigo cuando es visible en pantalla.
    public void ActivateMovement()
    {
        hasbeenVisible = true;
        rb2d.isKinematic = false;
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        movementPaused = false;
    }

    // Actualiza la velocidad del enemigo en cada frame, invirtiendo la dirección si es necesario.
    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            if (rb2d.velocity.x > -0.1f && rb2d.velocity.x < 0.1)
            {
                if (timer > 0.05f)
                {
                    speed = -speed;
                }
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
            // Actualiza la velocidad del enemigo, manteniendo la dirección y la velocidad vertical.
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);

            if (flipSprite)
            {
                if (rb2d.velocity.x > 0)
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

    // Pausa el movimiento del enemigo, guardando la dirección y velocidad actuales.
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

    // Reanuda el movimiento del enemigo con la dirección y velocidad guardadas.
    public void ContinueMovement()
    {
        if (movementPaused)
        {
            speed = defaultSpeed * currentDirection.x;
            rb2d.velocity = new Vector2(speed, lastVelocity.y);
            movementPaused = false;
        }
    }


    // Reanuda el movimiento del enemigo.
    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            rb2d.velocity = newVelocity;
            movementPaused = false;
        }
    }

    // Cambia la dirección del movimiento del enemigo, invirtiendo la velocidad (movimiento del eje).
    public void ChangeDirection()
    {
        speed = -speed;
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
    }
}
