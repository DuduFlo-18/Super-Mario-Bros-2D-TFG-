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

        if (Physics2D.Raycast(pieizq, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(pieder, Vector2.down, col2D.bounds.extents.y * 1.5f, groundLayer))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            mario.Hit();
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
            enemy.Stomped(transform);
            mover.BounceUp();
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
