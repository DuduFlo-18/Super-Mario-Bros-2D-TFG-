using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Mario : MonoBehaviour
{
    //Cada uno representa un estado de Mario
    public enum State {Default = 0, Super = 1, Fire = 2}
    State currentState = State.Default;
    public GameObject stompBox;
    Move mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2d;

    public GameObject fireballPrefab;
    public Transform shootPos;

    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;

    public bool isHurt;
    public float hurtTime;
    float hurtTimer;
    public bool isCrouched;
    //public GameObject headbox;

    public bool levelFinished;
    bool isDead;
  
    private void Awake()
    {
        mover = GetComponent<Move>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
        //Se encarga de activar o desactivar la hitbox de pisar si esta cayendo
            if(rb2d.velocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                stompBox.SetActive(false);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (colisiones.isGrounded)
                {
                    isCrouched = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Shoot();
            }
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }

            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }

        animaciones.Crouch(isCrouched);

        
//Se encarga de activar o desactivar la hitbox de salto si Mario esta cayendo
        // if (rb2d.velocity.y > 0)
        // {
        //     headbox.SetActive(true);
        // }
        // else
        // {
        //     headbox.SetActive(false);
        // }

        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     Time.timeScale = 0;
        //     animaciones.PowerUp();
        // }
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     Hit();
        // }
    }

    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead();
            }
            else
            {
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }
       
    }

    void StartHurt()
    {
        isHurt= true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);
    }

    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);
        
    }

    public void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            colisiones.Dead();
            mover.Dead();
            animaciones.Dead();
        }
    }

    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;
    }


    public void CatchItem(ItemType type)
    {
        switch(type)
        {
            case ItemType.MagicMushroom:
            if(currentState == State.Default)
            {
                animaciones.PowerUp();
                Time.timeScale = 0;
            }
                break;

            case ItemType.FireFlower:
                if(currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                }
                break;

            case ItemType.Coin:
                Debug.Log("Coin");
                break;

            case ItemType.Life:
                break;

            case ItemType.Star:
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                break;

            default:
                break;
        }
    }
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            GameObject newFireball = Instantiate(fireballPrefab, shootPos.position, Quaternion.identity);
            newFireball.GetComponent<Fireball>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }
    public bool IsBig()
    {
        return currentState != State.Default;
    }

    public void Goal()
    {
        mover.DownFlagPole();
        levelFinished = true;
    }
}
