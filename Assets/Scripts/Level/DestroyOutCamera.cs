using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutCamera : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    bool wasVisible;

    public float minDistance = 0;

    public GameObject parentObject;
    public bool onlyBack;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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
                // if (onlyBack && minDistance == 0)
                // {
                //     if (transform.position.x > Camera.main.transform.position.x)
                //     {
                //         return;
                //     }
                // }
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
