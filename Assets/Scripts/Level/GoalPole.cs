using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Funcionalidad del banderin en el final del nivel (Animaciones, puntuación, etc.)
public class GoalPole : MonoBehaviour
{
    //La posicion de la bandera la usaremos para animar la bandera al final del nivel (haciendo que baje)
    public Transform flag;
    //La posicion del final de la bandera, donde se queda al final de la animación
    public Transform bottom;

    //Velocidad de la bandera al bajar
    public float flagVelocity = 3f;

    // Deteccion de si la bandera ha bajado y mario puede ir hacia el castillo.
    bool downFlag;
    Move mover;

    // Prefab de puntos, para que al tocar la bandera se instancien los puntos que se suman a la puntuación del jugador.
    public GameObject pointPrefab;

    private void FixedUpdate()
    {
        if (downFlag)
        {
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - flagVelocity * Time.fixedDeltaTime);
            }
            else
            {
                mover.isFlagdown = true;
            }
        }
    }


    // Cuando Mario toca la bandera, se activa la animación de la bandera bajando y se calcula la puntuación que se le da al jugador.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Move>();
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            CalculateHeight(contactPoint.y);
        }
    }

    // Calcula la puntuación que se le da al jugador en función de la altura a la que ha tocado la bandera.
    void CalculateHeight(float marioPosition)
    {
       
    // Calculo de la altura de la bandera para determinar la puntuación.

        float size = GetComponent<BoxCollider2D>().bounds.size.y;
        //Debug.Log("Totalsize: " + size);

        float minPos1 = transform.position.y + (size - size / 5f); // Parte más alta = 5000 pts
        //Debug.Log("Min1: " + minPos1);

        float minPos2 = transform.position.y + (size - 2 * size / 5f); // Parte más alta = 2000 pts
        //Debug.Log("Min2: " + minPos2);

        float minPos3 = transform.position.y + (size - 3 * size / 5f); // Parte más alta = 1000 pts
        //Debug.Log("Min3: " + minPos3);

        float minPos4 = transform.position.y + (size - 4 * size / 5f); // Parte más alta = 400 pts
        //Debug.Log("Min4: " + minPos4);

    // Calcula la puntuación en función de la altura a la que ha tocado la bandera.
        int numPoints = 0;
        if (marioPosition >= minPos1)
        {
            numPoints = 5000;
        }
        else if (marioPosition >= minPos2)
        {
            numPoints = 2000;
        }
        else if (marioPosition >= minPos3)
        {
            numPoints = 800;
        }
        else if (marioPosition >= minPos4)
        {
            numPoints = 400;
        }
        else
        {
            numPoints = 200;
        }


        ScoreManager.instance.AddScore(numPoints);

        Vector2 positionPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoint = Instantiate(pointPrefab, positionPoints, Quaternion.identity);
        Points points = newFloatPoint.GetComponent<Points>();
        points.numPoints = numPoints;
        points.speed = flagVelocity;

        points.distance = flag.position.y - bottom.position.y;
        points.destroy = false;
    }
}
