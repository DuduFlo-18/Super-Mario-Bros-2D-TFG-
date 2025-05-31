using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Points : MonoBehaviour
{
    // Se crea un array de datos de puntos, que contiene el número de puntos y el sprite correspondiente. (Abajo se define la clase PointsData)
    public PointsData[] pointsData;
    public int numPoints = 0;
    public float distance = 2f;
    public float speed = 2f;
    public bool destroy = true;

    // Los puntos flotantes tienen animacion de subida, por lo que se les asigna una posición objetivo a la que se mueven.
    float targetPos;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Al mostrar los puntos, se selecciona el sprite correspondiente al número de puntos obtenidos.
    void Start()
    {
        ShowPoints(numPoints);
        targetPos = transform.position.y + distance;
    }

    // Este método se llama en cada frame para actualizar la posición del objeto de puntos flotantes.
    private void Update()
    {
        if (transform.position.y < targetPos)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
        }
        else
        {
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }

    // Este método se llama para mostrar los puntos obtenidos, seleccionando el sprite correspondiente al número de puntos.
    void ShowPoints(int pointsSelected)
    {
        for (int i = 0; i < pointsSelected; i++)
        {
            if (pointsData[i].numPoints == pointsSelected)
            {
                spriteRenderer.sprite = pointsData[i].sprite;
                break;
            }
        }
    }
}

// Clase que contiene los datos de los puntos, como el número de puntos y el sprite correspondiente.
[Serializable]
public class PointsData
{
    public int numPoints;
    public Sprite sprite;
}
