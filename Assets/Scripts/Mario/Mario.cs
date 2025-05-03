using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public enum State {Default = 0, Super = 1, Fire = 2}
    State currentState = State.Default;
    public GameObject stompBox;
    Move mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2d;

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
        if(rb2d.velocity.y < 0 && !isDead)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            animaciones.PowerUp();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }

    public void Hit()
    {
        if (currentState == State.Default)
        {
            Dead();
        }
        else
        {
            Time.timeScale = 0;
           animaciones.Hit();
        }
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
}
