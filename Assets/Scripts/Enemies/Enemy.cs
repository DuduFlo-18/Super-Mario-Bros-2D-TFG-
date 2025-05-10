using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AutoMovement automovement;
    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        automovement = GetComponent<AutoMovement>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Si chocan enemigos entre ellos, estos cambiaran la direccion
        if (collision.gameObject.layer == gameObject.layer)
        {
            automovement.ChangeDirection();
        }
    }
    public virtual void Stomped(Transform player)
    {

    }

    public virtual void HitFireBall()
    {
        FlipDie();
    } 

    public virtual void HitStarman()
    {
        FlipDie();
    }

    public virtual void HitBelowBlock()
    { 
        FlipDie();
    }

    public virtual void HitRollingShell()
    {
        FlipDie();
    }

    protected void FlipDie()
    {
        animator.SetTrigger("Flip");
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        if (automovement != null)
        {
            automovement.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
    }
}
