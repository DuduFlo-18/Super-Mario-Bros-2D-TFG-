using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    //Poder pasarle el jugador y que la camara le siga.
    public Transform target;

    // Desfase de la camara respecto al jugador. (Asi ponemos a mario en el lateral izquierdo de la pantalla)
    public float follow = 2.5f;

    // Limite izquierdo y derecho de la camara (necesita calcularse con el leftLimit / rightLimit).
    public float minPosX;
    public float maxPosX;

    // Limites de la camara, que se asignan desde el editor.
    public Transform leftLimit;
    public Transform rightLimit;

    // Variable para calcular el ancho de la camara.
    float camWidth;
    float lastPos;

    // Limites de la camara a la hora de jugar (esto lo usaremos para eliminar enemigos / objetos que no esten en pantalla).
    public Transform colLeftLimit;
    public Transform colRightLimit;

    //Variable para saber si la camara puede moverse o no.
    public bool canMove;

    // Inicializamos las variables y calculamos los limites de la camara.
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minPosX = leftLimit.position.x + camWidth;
        maxPosX = rightLimit.position.x - camWidth;
        lastPos = minPosX;

        colLeftLimit.position = new Vector2(transform.position.x - camWidth - 0.5f, colLeftLimit.position.y);
        colRightLimit.position = new Vector2(transform.position.x + camWidth + 0.5f, colRightLimit.position.y);
    }

    // Actualizamos la posicion de la camara en cada frame
    void Update()
    {
        if (target != null && canMove)
        {
            float newPosX = target.position.x + follow;
            //Limitar la camara para que no se salga de los limites
            newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        }
    }

    // Iniciar el seguimiento de un Mario
    public void StartFollow(Transform newTarget)
    {
        target = newTarget;
        float newPosX = target.position.x + follow;
        newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
        canMove = true;
    }

    public void UpdateRightLimit(float newLimit)
    {
        maxPosX = newLimit - camWidth;
        colRightLimit.position = new Vector2(maxPosX + 0.5f, colRightLimit.position.y);
    }
}
