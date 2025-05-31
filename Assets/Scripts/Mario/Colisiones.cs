using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de gestionar las colisiones de Mario con el entorno, incluyendo el suelo, enemigos y plataformas.
public class Colisiones : MonoBehaviour
{
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;


    BoxCollider2D col2D;
    Mario mario;
    Move mover;
    public LayerMask sideCollisions;

    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
        mario = GetComponent<Mario>();
        mover = GetComponent<Move>();
    }


// Logica para comprobar si Mario esta en el suelo, y asi poder saltar o no.
    public bool Grounded()
    {
        Vector2 pieizq = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 pieder = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);

        //Muestra visual de los bordes para saber si esta en contacto con el suelo. (Solo para debug)
        Debug.DrawRay(pieizq, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);
        Debug.DrawRay(pieder, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);

        if (Physics2D.Raycast(pieizq, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(pieder, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }

    private void FixedUpdate()
    {
        //Devuelve si esta colisionando con el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool CheckCollision(int direction)
    {
        //Devuelve si esta colisionando con el suelo
        return Physics2D.OverlapBox(col2D.bounds.center + Vector3.right * direction * col2D.bounds.extents.x, col2D.bounds.size * 0.25f, 0, sideCollisions);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Interaccion con enemigos
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (mario.isInvincible)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.HitStarman();
            }
            else
            {
                mario.Hit();
            }
        }

        //Interaccion con la Lava
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            //Si es invencible no le afecta (Estrella)
            if (!mario.isInvincible)
            {
                mario.Dead();
            }
        }


        //Interaccion con la barras de fuego (Nivel de Bowser)
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamagePlayer"))
        {
            //Si es invencible no le afecta (Estrella)
            if (!mario.isInvincible)
            {
                mario.Hit();
            }
        }

    }

    // Cambia el layer del jugador a "PlayerDead" para que no colisione con nada (solo sea visual)
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach (Transform transform in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        }
    }

    // Cambia el layer del jugador a "Player" para que pueda colisionar con los objetos del juego
    public void Respawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach (Transform transform in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    // Cambia el layer del jugador a "OnlyGround" para que no colisione con nada excepto el suelo (I-Frames)
    public void HurtCollision(bool activate)
    {
        if (activate)
        {
            gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    // Manejamos las colisiones con los enemigos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (mario.isInvincible)
            {
                enemy.HitStarman();
            }
            else
            {
                if (collision.CompareTag("Plant"))
                {
                    mario.Hit();
                }
                else
                {
                    enemy.Stomped(transform);
                    mover.BounceUp();
                }
            }
        }

    }

    //Si mario se encuentra encima de una plataforma, se asocia como padre para que se mueva con ella
    // Se le asocia como padre de la plataforma para que se mueva con ella
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") && isGrounded)
        {
            transform.parent = collision.transform;
        }
    }

    // Al salir de la colision con una plataforma, se desasocia del padre para que no se mueva con la plataforma
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            transform.parent = null;
            //Importante para que no se elimine entre niveles
            DontDestroyOnLoad(gameObject);
        }
    }
}
