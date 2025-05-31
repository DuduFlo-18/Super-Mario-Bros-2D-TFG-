using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Funcionalidad de la animación de imágenes, que cambia entre diferentes sprites en un intervalo de tiempo determinado. (Usado en el HUD para mostrar una animación de monedas)
public class ImageAnimation : MonoBehaviour
{

    // Creamos un array de sprites para que se puedan asignar diferentes imágenes a la animación.
    public Sprite[] sprites;

    // Tiempo entre cada cambio de imagen en segundos.
    public float frameTime = 0.1f;

    Image imagen;
    int animationFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        imagen = GetComponent<Image>();
        // Se llama a la función ChangeImage cada frameTime para cambiar la imagen.
        InvokeRepeating("ChangeImage", frameTime, frameTime);
    }

    // Realiza el cambio de imagen en el sprite del componente Image. 
    void ChangeImage()
    {
        imagen.sprite = sprites[animationFrame];
        animationFrame++;
        // Si se ha llegado al final del array de sprites, se reinicia el contador para que vuelva a empezar desde el primer sprite.
        if (animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }
    }
}
