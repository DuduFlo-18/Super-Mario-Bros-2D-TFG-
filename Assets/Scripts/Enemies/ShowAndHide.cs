using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    // Objecto que se moverá entre dos puntos
    public GameObject objectToMove;

    // Puntos entre los que se moverá el objeto
    public Transform showPoint;
    public Transform hidePoint;


    // Tiempo de espera antes de mostrar o esconder (Planta Piraña)
    public float waitForHide;
    public float waitForShow;

    // Velocidades de movimiento al mostrarse y esconderse
    public float speedShow;
    public float speedHide;


    // Tiempo de espera para mostrar y esconder
    float timershow;
    float timerhide;

    float speed;
    Vector2 targetPoint;

    void Start()
    {
        targetPoint = hidePoint.position;
        speed = speedHide;
        timerhide = 0;
        timershow = 0;
    }

    //Logica de movimiento (Planta Piraña)
    void Update()
    {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, targetPoint, speed*Time.deltaTime);

        if (Vector2.Distance(objectToMove.transform.position, hidePoint.position) < 0.1f)
        {
            //Si esta escondido
            timershow += Time.deltaTime;
            if (timershow >= waitForShow && !Locked())
            {
                targetPoint = showPoint.position;
                speed = speedShow;
                timershow = 0;
            }
        }

        else if (Vector2.Distance(objectToMove.transform.position, showPoint.position)< 0.1f)
        {
            //Si la planta se esta mostrando
            timerhide += Time.deltaTime;
            if (timerhide >= waitForShow)
            {
                targetPoint = hidePoint.position;
                speed = speedHide;
                timerhide= 0;
            }
        }
    }

    // Se creo un cubo en el que si Mario esta dentro de este cubo, la planta no podrá salir del estado Hidden
    bool Locked()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.up, Vector2.one, 0);
    }

    // Este metodo es solo para poder visualizar el cubo de colision en el editor del Unity, no tiene efecto en el juego.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector2.one);
    }
}
