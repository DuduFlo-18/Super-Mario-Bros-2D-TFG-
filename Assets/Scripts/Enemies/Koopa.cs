using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    bool isHidden;

    //Tiempo metido en el caparazon
    public float maxStoppedTime;
    float stoppedTimer;

    public float rollingSpeed; 

    //Si esta rodando no podrá salir del caparazon
    public bool isRolling;
    protected override void Update()
    {
        base.Update();
        //Comprueba si el caparazon esta quieto y escondido
        if (isHidden && rb2d.velocity.x == 0f)
        {
            stoppedTimer += Time.deltaTime;
            if (stoppedTimer >= maxStoppedTime)
            {
                ResetMove();
            }
        }
    }
    
    // Logica de si Mario pisa al caparazon
    public override void Stomped(Transform player)
    {
        isRolling = false;
        AudioManager.instance.PlayStomp();
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
                //Si mario esta a la izquierda o a la derecha del Caparazon, se lanza en esa dirección
                if (player.position.x < transform.position.x)
                {
                    automovement.speed = rollingSpeed;
                }
                else
                {
                    automovement.speed = -rollingSpeed;
                }
                automovement.ContinueMovement(new Vector2(automovement.speed, 0));
                isRolling = true;
            }
        }

        // Logica para destruir el Koopa si este sale fuera de la pantalla
        DestroyOutCamera destroyOutCamera = GetComponent<DestroyOutCamera>();
        if (isRolling)
        {
            destroyOutCamera.onlyBack = false;
        }
        else
        {
            destroyOutCamera.onlyBack = true;
        }

        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

        //Para que se salga de la hitbox temporalmente
        Invoke("ResetLayer", 0.1f);
        stoppedTimer = 0;
    }

    //Si se choca con un caparazon rodante en movimiento elimina al enemigo, sino cambiará la dirección del caparazon
    public override void HitRollingShell()
    {
        if (!isRolling)
        {
            FlipDie();
        }
        else
        {
            automovement.ChangeDirection();
        }
    }

    // Metodo para reiniciar  la capa del Koopa y considerarlo como enemigo de nuevo
    void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    // Reinicia el movimiento del Koopa si este se queda quieto durante un tiempo
    void ResetMove()
    {
        automovement.ContinueMovement();
        isHidden = false;
        animator.SetBool("Hidden", isHidden);
        stoppedTimer = 0;
    }
    
    // Si el Koopa esta rodando y choca con un enemigo, este recibe el golpe. Sino, se comporta como un enemigo normal y chocan.
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRolling)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //                Debug.Log("Koopa Hit");
                collision.gameObject.GetComponent<Enemy>().HitRollingShell();
            }
            else if (!isHidden)
            {
                base.OnCollisionEnter2D(collision);
            }
        }
    }
}
