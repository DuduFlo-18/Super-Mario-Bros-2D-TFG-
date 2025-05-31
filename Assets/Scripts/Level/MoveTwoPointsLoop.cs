using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script permite mover un objeto entre dos puntos de forma continua en bucle, usado para la plataforma flotante.
public class MoveTwoPointsLoop : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    Vector3 currentTarget;
    Vector3 startPoint;
    Vector3 endPoint;
    // Inicializa las variables startPoint y endPoint con las posiciones de los puntos A y B al inicio del juego.
    void Start()
    {
        startPoint = pointA.position;
        endPoint = pointB.position;
        currentTarget = endPoint;
    }

    // Actualiza la posición del objeto en cada frame, moviéndolo hacia el target actual.
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);
        if (transform.position == currentTarget)
        {
            if (currentTarget == endPoint)
            {
                currentTarget = startPoint;
            }
            else
            {
                currentTarget = endPoint;
            }
        }
    }
}
