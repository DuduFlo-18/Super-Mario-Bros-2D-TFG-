using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de destruir los objetos que no estan visibles en la camara si estos han sido vistos en algun momento (En el Editor se rompe un poco este script).
public class DestroyOutCamera : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool wasVisible;

// Distancia minima a la que el objeto se destruye si no esta visible
    public float minDistance = 0;

    public GameObject parentObject;

    // Si es true, solo se destruye si el objeto esta por detras de la camara (esto es util para objetos que se mueven hacia la derecha y no queremos que se destruyan si estan delante de la camara).
    public bool onlyBack;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Comprueba si es visible en la camara y si no lo es, destruye el objeto si ha sido visible en algun momento.
    void Update()
    {
        if (spriteRenderer.isVisible)
        {
            wasVisible = true;
        }
        else
        {
            if (wasVisible)
            {
                //Dejamos que el objeto se destruya si no esta visible
                if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance)
                {
                    if (onlyBack)
                    {
                        if (transform.position.x > Camera.main.transform.position.x)
                        {
                            return;
                        }
                    }

                    if (parentObject == null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(parentObject);
                    }
                }
            }
        }
    }
}
