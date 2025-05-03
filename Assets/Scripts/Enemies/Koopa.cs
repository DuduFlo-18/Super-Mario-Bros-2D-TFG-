using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    bool isHidden;
    public float maxStoppedTime;
    float stoppedTimer;

    public float rollingSpeed; 
    protected override void Update()
    {
        base.Update();
        if(isHidden && rb2d.velocity.x == 0f)
        {
         stoppedTimer += Time.deltaTime;   
         if (stoppedTimer >= maxStoppedTime)
         {
            ResetMove();
         }
        }
    }
    public override void Stomped(Transform player)
    {
        if (!isHidden)
        {
            isHidden = true;
            animator.SetBool("Hidden", isHidden);
            automovement.PauseMovement();
        }

        else
        {
            if (Mathf.Abs(rb2d.velocity.x) > 0f)
            {
                automovement.PauseMovement();
            }
            else
            {
            //Si mario esta a la izquierda o a la derecha del Caparazon
                if (player.position.x < transform.position.x)
                {
                    automovement.speed = rollingSpeed;
                }
                else
                {
                    automovement.speed  = -rollingSpeed;
                }
                automovement.ContinueMovement(new Vector2(automovement.speed,0));
            }

        
        }

        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

    //Para que se salga de la hitbox temporalmente
        Invoke("ResetLayer", 0.1f);
        stoppedTimer = 0;
    }

    void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void ResetMove()
    {
        automovement.ContinueMovement();
        isHidden = false;
        animator.SetBool("Hidden", isHidden);
        stoppedTimer = 0;
    }

}
