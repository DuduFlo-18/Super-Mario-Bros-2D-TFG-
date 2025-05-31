using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    public int points;
    protected Animator animator;
    protected AutoMovement automovement;
    protected Rigidbody2D rb2d;

    public GameObject floatPointsPrefab;

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        automovement = GetComponent<AutoMovement>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Si chocan enemigos entre ellos, estos cambiaran la direccion
        if (collision.gameObject.layer == gameObject.layer)
        {
            automovement.ChangeDirection();
        }
    }

    // Metodo virtual que puede ser sobreescrito por las subclases
    public virtual void Stomped(Transform player) { }
    
    // Golpeado por la bola de fuego
    public virtual void HitFireBall()
    {
        FlipDie();
    } 

    // Golpeado por Mario modo Estrella
    public virtual void HitStarman()
    {
        FlipDie();
    }

    // Este metodo mataba al enemigo si Mario lo golpeaba desde abajo del bloque en el que estaba situado el Enemigo.
    public virtual void HitBelowBlock()
    {
        FlipDie();
    }

    // Si el enemigo es golpeado por un caparazón rodando, se invoca este metodo.
    public virtual void HitRollingShell()
    {
        FlipDie();
    }

    //Metodo que se invoca para eliminar al enemigo.
    protected void FlipDie()
    {
        AudioManager.instance.PlayFlipDie();
        animator.SetTrigger("Flip");
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        if (automovement != null)
        {
            automovement.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
        Dead();
    }


    // Metodo que muestra el puntaje del enemigo al morir y lo añade al ScoreManager. (Muestra animación con puntos flotantes)
    protected void Dead()
    {
        ScoreManager.instance.AddScore(points);
        GameObject newFloatPoint = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
        Points floatPoints = newFloatPoint.GetComponent<Points>();
        floatPoints.numPoints = points;
    }
}
