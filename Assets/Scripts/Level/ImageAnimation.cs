using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

    public Sprite[] sprites;
    public float frameTime = 0.1f;

    Image imagen;
    int animationFrame = 0;
    // Start is called before the first frame update
    void Start()
    {
        imagen = GetComponent<Image>();
        InvokeRepeating("ChangeImage", frameTime, frameTime);
    }

    void ChangeImage()
    {
        imagen.sprite = sprites[animationFrame];
        animationFrame++;
        if (animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }
    }
}
