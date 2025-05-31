using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de establecer la resolucion de aspecto de la camara.
public class CameraAspect : MonoBehaviour
{
    public Vector2 targetAspect = new Vector2(16f, 15f);

    void Awake()
    {
        float targetaspect = targetAspect.x / targetAspect.y;
        float screenaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = screenaspect / targetaspect;
        float scalewidth = targetaspect / screenaspect;

        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;
        rect.width = scalewidth;
        rect.height = 1f;
        rect.x = (1f - scalewidth) / 2f;
        rect.y = 0f;
        camera.rect = rect;
    }
}
