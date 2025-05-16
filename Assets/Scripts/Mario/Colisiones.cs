using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public bool Grounded()
    {
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

        Vector2 pieizq = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 pieder = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);

//Muestra visual de los bordes para saber si esta en contacto con el suelo.
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
    }

    public bool CheckCollision(int direction)
    {
        //Devuelve si esta colisionando con el suelo
        return Physics2D.OverlapBox(col2D.bounds.center + Vector3.right * direction * col2D.bounds.extents.x, col2D.bounds.size * 0.5f, 0, sideCollisions);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

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

        // if(collision.gameObject.CompareTag("Pipe"))
        // Debug.Log("Colision Enter: " + collision.gameObject.name);
        // else if (collision.gameObject.CompareTag("Ground")) 
        // {
        //     Debug.Log("Empezamos a tocar el suelo");
        // }
    }

    public void Dead() 
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach(Transform transform in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        }
    }
    public void Respawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach(Transform transform in transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

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

    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Pipe"))
    //     Debug.Log("Colision Stay: " + collision.gameObject.name);
    // } 

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Pipe"))
    //     Debug.Log("Colision Exit: " + collision.gameObject.name);
    //     else if (collision.gameObject.CompareTag("Ground")) 
    //     {
    //         Debug.Log("Terminamos de tocar el suelo");
    //     }
    // } 

     private void OnTriggerEnter2D(Collider2D collision)
    {
        // PlayerHit playerHit = collision.GetComponent<PlayerHit>();
        // if (playerHit != null)
        // {
        //     playerHit.Hit();
        //     mover.BounceUp();
        // }

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


    //     if (collision.gameObject.CompareTag("Ground")) 
    //      {
    //          Debug.Log("Empezamos a tocar el suelo");
    //      }
    }

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground")) 
    //      {
    //          Debug.Log("Dejamos de tocar el suelo");
    //      }
    // }


}
