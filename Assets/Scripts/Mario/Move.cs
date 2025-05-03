using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    enum Direction {Left = -1, None = 0, Right = 1};
    Direction currentDirection = Direction.None;

//Valores de Movimiento X
    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0f;

//Valores de Salto
    public float jumpForce;
    public float maxJumpTime = 1f;
    public bool isJumping;
    float jumpTimer = 0;
    float defaultGravity;

    public bool isSkidding;
    public Rigidbody2D rb2d;
    Colisiones colisiones;

    public bool inputMoveEnabled = true;
    Animaciones animaciones;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
    }

    void Start()
    {
        defaultGravity = rb2d.gravityScale;
    }

    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);
        if(isJumping)
        {
            if(rb2d.velocity.y > 0f)
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    jumpTimer += Time.deltaTime;
                }
                if(Input.GetKeyUp(KeyCode.Space))
                {
                    if(jumpTimer < maxJumpTime)
                    {
                        rb2d.gravityScale = defaultGravity*3f;
                    }
                }
            }

            else
            {
                rb2d.gravityScale = defaultGravity;
                if (grounded)
                {
                    isJumping = false;
                    jumpTimer = 0;
                    animaciones.Jumping(false);
                }
            }
        }

        
//Prueba de Movimiento 1 
         currentDirection = Direction.None;
         if (inputMoveEnabled)
         {
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                if (grounded)
                {
                    Jump();
                }
            }

            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
            {
                currentDirection = Direction.Left;
            }

            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
            {
                currentDirection = Direction.Right;
            }
         }
    }

    private void FixedUpdate()
    {
        isSkidding = false;
    //Comprueba velocidad actual en el eje X
        currentVelocity = rb2d.velocity.x;
        if(currentDirection > 0) 
        {
            if (currentVelocity < 0)
            {
                currentVelocity += (acceleration + friction) * Time.deltaTime;
                isSkidding = true;
            }
            else if(currentVelocity < maxVelocity)
            {
                currentVelocity += acceleration*Time.deltaTime;
                transform.localScale = new Vector2(1,1);
            }
        }
        else if (currentDirection < 0)
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= (acceleration + friction) * Time.deltaTime;
                isSkidding = true;
            }
            else if(currentVelocity >  -maxVelocity)
            {
                currentVelocity -= (acceleration*Time.deltaTime);
                transform.localScale = new Vector2(-1,1);
            }
        }
        else 
        {
            if(currentVelocity > 1f)
            {
                currentVelocity-= friction * Time.deltaTime;
            }
            else if(currentVelocity < -1f) 
            {
                currentVelocity += friction * Time.deltaTime;
            }
            else 
            {
                currentVelocity = 0;
            }
        }

        Vector2 velocity = new Vector2(currentVelocity, rb2d.velocity.y);
        rb2d.velocity = velocity;
        animaciones.Velocity(currentVelocity);
        animaciones.Skid(isSkidding);
    }
    void Jump() 
    {
        if(!isJumping)
        {
        isJumping = true;
        Vector2 fuerza = new Vector2(0, jumpForce);
        rb2d.AddForce(fuerza, ForceMode2D.Impulse);
        animaciones.Jumping(true);
        }
    }

    void MoveRight() 
    {
        Vector2 velocity = new Vector2(1f, 0);
        rb2d.velocity = velocity;
    }

    public void Dead()
    {
        inputMoveEnabled = false;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 1;
        rb2d.AddForce(Vector2.up*5f, ForceMode2D.Impulse);
    }

    public void BounceUp()
    {
        //Se para el movimiento para que no se multiplique el impulso con el salto
        rb2d.velocity = Vector2.zero;
        //Vector2 forceUp = new Vector2(0, 10f);
        rb2d.AddForce(Vector2.up*10f, ForceMode2D.Impulse);

    }
}
