using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//La creo fuera de la clase para poder acceder desde cualquier archivo
public enum ItemType { MagicMushroom, FireFlower, Coin, Life, Star }
public class Item : MonoBehaviour
{

    public ItemType type;
    bool isCatched;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCatched)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<Mario>().CatchItem(type);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    //Codigo para que identifique la Hitbox del Mario unicamente, y no toque el item varias veces
        if (!isCatched)
        {
            Mario mario = collision.gameObject.GetComponent<Mario>();
            if (mario != null)
            {
                isCatched = true;
                mario.CatchItem(type);
                Destroy(gameObject);
            }
        }
    }
}
