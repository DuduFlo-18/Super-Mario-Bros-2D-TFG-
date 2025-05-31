using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de animar las barras de fuego en el nivel del Castillo de Bowser
public class FireBar : MonoBehaviour
{
    // Velocidad de rotacion de la barra de fuego
    public float rotationspeed = 50f;

    // Inicializa la velocidad de rotacion de la barra de fuego
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
    }
}
