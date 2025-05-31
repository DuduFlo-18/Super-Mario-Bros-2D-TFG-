using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BowserFire : MonoBehaviour
{
    // Este script controla el disparo de fuego de Bowser.
    // Se encarga de la dirección, velocidad y animación del disparo.
    public float fireDirection;
    public float fireSpeed = 10f;

    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        fireSpeed *= fireDirection;
        //Al reves porque el sprite esta mirando a la derecha
        transform.localScale = new Vector3(-fireDirection, 1, 1);
        rb2d.velocity = new Vector2(fireSpeed, 0);
        StartCoroutine(FireAnimation());
    }
    
    //Le añadimos una animacion (giramos el sprite por el eje Y)
    IEnumerator FireAnimation()
    {
        while (true)
        {
            spriteRenderer.flipY = !spriteRenderer.flipY;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
