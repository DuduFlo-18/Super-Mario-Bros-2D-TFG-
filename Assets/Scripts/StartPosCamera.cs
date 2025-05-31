using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script gestiona la posición inicial de la cámara en un juego 2D, asegurando que la cámara se sitúe a una distancia adecuada del punto de inicio del nivel.
public class StartPosCamera : MonoBehaviour
{
    public Transform startPos;
    void Start()
    {
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float camPos = startPos.position.x + camWidth;
        transform.position = new Vector3(camPos, transform.position.y, transform.position.z);
    }
}
