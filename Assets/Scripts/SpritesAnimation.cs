using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.1f;
    float timer = 0f;
    int animationFrame = 0;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
       //Animation();

       //PROBLEMA SOLUCIONADO
       StartCoroutine(Animation());
    }

    // Update is called once per frame
    // void Update()
    // {
    //     timer += Time.deltaTime;
    //     if (timer >= frameTime)
    //     {
    //         //Cambio de sprites
    //         animationFrame++;
            
    //         //Controlamos la posicion del array
    //         if (animationFrame >= sprites.Length)
    //         {
    //             animationFrame = 0;
    //         }

    //         spriteRenderer.sprite = sprites[animationFrame];
    //         timer = 0;
    //     }
    // }

    IEnumerator Animation()
    {
        while (true)
        {
            Debug.Log("Animation Phase: "+ animationFrame);
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
}
