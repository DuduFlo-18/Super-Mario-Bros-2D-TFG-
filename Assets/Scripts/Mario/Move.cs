using System;
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

  //  public GameObject headBox;
    Animaciones animaciones;


    bool isClimbingFlagPole = false;
    public float climbPoleSpeed = 5;
    public bool isFlagdown;

    bool isAutoWalking;
    public float autoWalkSpeed = 5;
    Mario mario;
    

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
    }

    void Start()
    {
        defaultGravity = rb2d.gravityScale;
    }

    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);

        if (LevelManager.instance.levelFinished)
        {
            if (grounded && isClimbingFlagPole)
            {
                StartCoroutine(JumpOffFlagPole()); 
            }
        }

        else
        {
 //           headBox.SetActive(false);
            if(isJumping)
        {
            if(rb2d.velocity.y > 0f)
            {
 //               headBox.SetActive(true);
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
        
    }

    private void FixedUpdate()
    {
        if (LevelManager.instance.levelFinished)
        {
            if (isClimbingFlagPole)
            {
                rb2d.MovePosition(rb2d.position + Vector2.down * climbPoleSpeed * Time.fixedDeltaTime);
            }
            else if (isAutoWalking)
            {
                Vector2 velocity = new Vector2(currentVelocity, rb2d.velocity.y);
                rb2d.velocity = velocity;
                animaciones.Velocity(Math.Abs(currentVelocity));
            }
        }
        else
        {
                isSkidding = false;
        //Comprueba velocidad actual en el eje X
            currentVelocity = rb2d.velocity.x;
            
            if (colisiones.CheckCollision((int)currentDirection))
            {
                currentVelocity = 0;
            }
            else
            {
                 if (currentDirection > 0)
            {
                if (currentVelocity < 0)
                {
                    currentVelocity += (acceleration + friction) * Time.deltaTime;
                    isSkidding = true;
                }
                else if (currentVelocity < maxVelocity)
                {
                    currentVelocity += acceleration * Time.deltaTime;
                    transform.localScale = new Vector2(1, 1);
                }
            }
            else if (currentDirection < 0)
            {
                if (currentVelocity > 0)
                {
                    currentVelocity -= (acceleration + friction) * Time.deltaTime;
                    isSkidding = true;
                }
                else if (currentVelocity > -maxVelocity)
                {
                    currentVelocity -= (acceleration * Time.deltaTime);
                    transform.localScale = new Vector2(-1, 1);
                }
            }
            else
            {
                if (currentVelocity > 1f)
                {
                    currentVelocity -= friction * Time.deltaTime;
                }
                else if (currentVelocity < -1f)
                {
                    currentVelocity += friction * Time.deltaTime;
                }
                else
                {
                    currentVelocity = 0;
                }
            }
            }
           
            if (mario.isCrouched)
            {
                currentVelocity = 0;
            }
            Vector2 velocity = new Vector2(currentVelocity, rb2d.velocity.y);
            rb2d.velocity = velocity;
            animaciones.Velocity(currentVelocity);
            animaciones.Skid(isSkidding);
        }
    }
    void Jump() 
    {
        //Logica de Sonido
        if (mario.IsBig())
        {
            AudioManager.instance.PlayBigJump();
        }
        else
        {
            AudioManager.instance.PlayJump();
        }
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

    public void Respawn()
    {
        inputMoveEnabled = true;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = defaultGravity;
        transform.localScale = new Vector2(1, 1);
    }

    public void BounceUp()
    {
        //Se para el movimiento para que no se multiplique el impulso con el salto
        rb2d.velocity = Vector2.zero;
        //Vector2 forceUp = new Vector2(0, 10f);
        rb2d.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

    }

    public void DownFlagPole()
    {
        inputMoveEnabled = false;
        rb2d.isKinematic = true;
        rb2d.velocity = new Vector2(0, -0f);
        isClimbingFlagPole = true;
        isJumping = false;
        animaciones.Jumping(false);
        animaciones.Climb(true);
        transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
    }

    IEnumerator JumpOffFlagPole()
    {
        isClimbingFlagPole = false;
        rb2d.velocity = Vector2.zero;
        animaciones.Pause();
        yield return new WaitForSeconds(0.25f);
    //Esperamos a quee la bandera baje
        while (!isFlagdown)
        {
            yield return null;
        }

        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.25f);

        animaciones.Climb(false);
        rb2d.isKinematic = false;
        animaciones.Continue();
        GetComponent<SpriteRenderer>().flipX = false;
        isAutoWalking = true;
        currentVelocity = autoWalkSpeed;
    }
}
