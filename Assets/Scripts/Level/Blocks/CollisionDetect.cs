using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de detectar la colision del bloque con la cabeza de Mario.
public class CollisionDetect : MonoBehaviour
{
    Block block;
    private void Awake()
    {
        block = GetComponentInParent<Block>();
    }

    // Al entrar en colision con el bloque, se comprueba si Mario es grande o no y se realiza una acccion diferente.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HeadMario"))
        {
            collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.GetComponentInParent<Mario>().IsBig())
            {
                block.bouncing = false;
                block.HeadCollision(true);
            }
            else
            {
                block.bouncing = false;
                block.HeadCollision(false);
            }
        }
    }
}
