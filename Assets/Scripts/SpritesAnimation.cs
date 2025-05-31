using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script gestiona la animación de sprites en Unity, permitiendo reproducir una secuencia de sprites como una animación.
public class SpritesAnimation : MonoBehaviour
{
    // Permitimos que el usuario asigne una serie de sprites a la animación
    public Sprite[] sprites;
    public float frameTime = 0.1f;
    int animationFrame = 0;

    public bool stop;
    public bool loop = true;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }

    //Coroutine que gestiona la animación de los sprites
    IEnumerator Animation()
    {
        if (loop)
        {
            while (!stop)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;

                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                //Que vuelva al siguiente frame
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject);
        }
    }
}
