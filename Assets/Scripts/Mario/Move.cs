using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Usamos esta biblioteca para detectar eventos de entrada del usuario (Botones táctiles)
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 };
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
    SpriteRenderer spriterenderer;

    public bool moveConnectionComplete = true;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriterenderer = GetComponent<SpriteRenderer>();
        defaultGravity = rb2d.gravityScale;
    }

    void Start()
    {
        //Problemas con la gravedad, lo muevo al Awake
        //defaultGravity = rb2d.gravityScale;
    }

    void Update()
    {
        if (GameManager.instance.isGameOver || Mario.instance == null || Mario.instance.isDead)
        {
            animaciones.Grounded(true);
            return;
        }
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);


        //Comprobamos si el jugador esta en el suelo (confundia la animacion de salto con la de caida)
        if (grounded)
        {
            animaciones.Jumping(isJumping);
        }

        if (LevelManager.instance.levelFinished)
        {
            if (grounded && isClimbingFlagPole)
            {
                StartCoroutine(JumpOffFlagPole());
            }
        }

        else
        {

            // Prueba de Salto 1 (Inputs Normales)
            // if (isJumping)
            // {
            //     if (rb2d.velocity.y > 0f)
            //     {
            //         //               headBox.SetActive(true);
            //         if (Input.GetKey(KeyCode.Space))
            //         {
            //             jumpTimer += Time.deltaTime;
            //         }
            //         if (Input.GetKeyUp(KeyCode.Space))
            //         {
            //             if (jumpTimer < maxJumpTime)
            //             {
            //                 rb2d.gravityScale = defaultGravity * 3f;
            //             }
            //         }
            //     }

            //     else
            //     {
            //         rb2d.gravityScale = defaultGravity;
            //         isJumping = false;
            //         jumpTimer = 0;

            //         //Prueba de Salto fallida, mala colision con el Tilemap
            //         // if (grounded)
            //         // {
            //         //     isJumping = false;
            //         //     jumpTimer = 0;
            //         //     animaciones.Jumping(false);
            //         // }
            //     }
            // }

            // Prueba de Salto 2 (Inputs para mando y teclado)
            if (isJumping)
            {
                if (rb2d.velocity.y > 0f)
                {
                    if (Input.GetButton("Jump")) // detecta espacio y mando
                    {
                        jumpTimer += Time.deltaTime;
                    }

                    if (Input.GetButtonUp("Jump"))
                    {
                        if (jumpTimer < maxJumpTime)
                        {
                            rb2d.gravityScale = defaultGravity * 3f;
                        }
                    }
                }
                else
                {
                    rb2d.gravityScale = defaultGravity;
                    isJumping = false;
                    jumpTimer = 0;
                }
            }


        //Prueba de Movimiento 1 - Inputs Normales (Solo Teclado)
            currentDirection = Direction.None;
            // if (inputMoveEnabled)
            // {
            //     if (Input.GetKeyDown(KeyCode.Space))
            //     {
            //         if (grounded)
            //         {
            //             Jump();
            //         }
            //     }

            //     if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            //     {
            //         currentDirection = Direction.Left;
            //     }

            //     if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            //     {
            //         currentDirection = Direction.Right;
            //     }
            // }

        //Prueba de Movimiento 2 - Inputs para Mando y teclado
            float m = InputTranslator.Horizontal;

            if (InputTranslator.Jump && grounded)
            {
                Jump();
            }

            if (m < 0) currentDirection = Direction.Left;
            else if (m > 0) currentDirection = Direction.Right;

        }

    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        
    // Movimiento de Mario al tocar el Banderín
        // if (LevelManager.instance.levelFinished)
        // {
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
        //}

    // Movimiento y lógica del Mario 
        else if (!rb2d.isKinematic && !LevelManager.instance.levelFinished && inputMoveEnabled)
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
                //Si la direccion es positiva, movemos a la derecha
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
                //Si la direccion es negativa, movemos a la izquierda
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
                //Si no hay direccion, aplicamos friccion
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
    }


    // Metodo para saltar, se llama desde el Update cuando se pulsa el boton de salto (usa un condicional para evitar que suene varias veces el salto al jugar en Móvil)
    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2d.AddForce(fuerza, ForceMode2D.Impulse);
            animaciones.Jumping(true);

            // Reproducir sonido SOLO cuando inicia el salto
            if (mario.IsBig())
            {
                AudioManager.instance.PlayBigJump();
            }
            else
            {
                AudioManager.instance.PlayJump();
            }
        }
    }

    void MoveRight()
    {
        Vector2 velocity = new Vector2(1f, 0);
        rb2d.velocity = velocity;
    }

    // Logica de movimiento cuando Mario muere
    public void Dead()
    {
        inputMoveEnabled = false;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 1;
        rb2d.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

    // Logica de movimiento cuando Mario resucita
    public void Respawn()
    {
        isAutoWalking = false;
        inputMoveEnabled = true;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = defaultGravity;
        transform.localScale = new Vector2(1, 1);
    }

    // Logica de rebote al pisar a un enemigo
    public void BounceUp()
    {
        //Se para el movimiento para que no se multiplique el impulso con el salto
        rb2d.velocity = Vector2.zero;
        //Vector2 forceUp = new Vector2(0, 10f);
        rb2d.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    // Logica de movimiento al tocar el banderin
    public void DownFlagPole()
    {
        if (!isClimbingFlagPole)
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
    }

    // Corutina para saltar del banderin y dirigirse a la meta
    IEnumerator JumpOffFlagPole()
    {
        isAutoWalking = false;
        isClimbingFlagPole = false;
        rb2d.velocity = Vector2.zero;
        animaciones.Pause();
        yield return new WaitForSeconds(0.25f);
        //Esperamos a que la bandera baje
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
        AudioManager.instance.PlayLevelCompleted();
    }

    // Este metodo hace que Mario no pueda moverse de forma manual, y se mueve automaticamente hacia la derecha (Creado para cuando se termina un nivel)
    public void AutoWalk()
    {
        isAutoWalking = true;
        rb2d.isKinematic = false;
        inputMoveEnabled = false;
        currentVelocity = autoWalkSpeed;
    }

    // Este metodo hace que Mario se mueva automaticamente hacia una direccion concreta, y no pueda moverse de forma manual. (Creado para cuando se entra en una tuberia)
    public void AutomoveConnection(ConnectDirection direction)
    {
        isAutoWalking = false;
        moveConnectionComplete = false;
        inputMoveEnabled = false;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        spriterenderer.sortingOrder = -100;

        switch (direction)
        {
            case ConnectDirection.Up:
                //moveConnectionComplete = true;
                StartCoroutine(AutoMoveConnectionUp());
                break;
            case ConnectDirection.Right:
                //moveConnectionComplete = true;
                StartCoroutine(AutoMoveConnectionRight());
                break;
            //No hacemos uso del Left asi que lo dejamos como un caso vacio
            case ConnectDirection.Left:
                moveConnectionComplete = true;
                break;
            case ConnectDirection.Down:
                //moveConnectionComplete = true;
                StartCoroutine(AutoMoveConnectionDown());
                break;
        }
    }

    // Metodo para recuperar el movimiento de Mario, se usa cuando termina de moverse por una tuberia o al terminar un nivel.
    public void ResetMove()
    {
        rb2d.isKinematic = false;
        spriterenderer.sortingOrder = 20;
        inputMoveEnabled = true;
    }

    // Este metodo se encarga de mover a Mario automaticamente hacia abajo (Usado para las tuberias)
    IEnumerator AutoMoveConnectionDown()
    {
        float targetdown = transform.position.y - spriterenderer.bounds.size.y;
        while (transform.position.y > targetdown)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionComplete = true;
    }

    // Este metodo se encarga de mover a Mario automaticamente hacia arriba (Usado para salir de las tuberias)
    IEnumerator AutoMoveConnectionUp()
    {
        float targetUp = transform.position.y + spriterenderer.bounds.size.y;
        while (transform.position.y < targetUp)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionComplete = true;
    }

    // Este metodo se encarga de mover a Mario automaticamente hacia la derecha (Usado para salir de las tuberias)
    IEnumerator AutoMoveConnectionRight()
    {
        float targetRight = transform.position.x + spriterenderer.bounds.size.x * 2;
        //Conseguir ese sprite moviendose al meterse en la tuberia
        animaciones.Velocity(1);
        while (transform.position.x < targetRight)
        {
            transform.position += Vector3.right * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //Quitamos la velocidad para que no se quede el sprite moviendose
        animaciones.Velocity(0);
        moveConnectionComplete = true;
    }

    // Este metodo se encarga de detener el movimiento de Mario, se usa cuando se quiere parar el movimiento manualmente.
    public void StopMove()
    {
        inputMoveEnabled = false;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        isAutoWalking = false;
        animaciones.Velocity(0);
    }
}