using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// Funcionalidad de los items que Mario puede recoger en el juego (setas, flores, monedas, vidas, estrellas, etc.)


//La creo fuera de la clase para poder acceder desde cualquier archivo
public enum ItemType { MagicMushroom, FireFlower, Coin, Life, Star }
public class Item : MonoBehaviour
{
    public int points;
    public ItemType type;

    // Esta variable la añadi para no poder pillar varias veces el mismo item, ya que al ser un trigger, al tocarlo varias veces, se ejecuta el OnTriggerEnter2D varias veces.
    bool isCatched;
    
    public Vector2 startVelocity;

    // Componente de movimiento automático, que permite que el item se mueva automáticamente en una dirección determinada.
    AutoMovement autoMovement;

    // Prefab de puntos que se instancian al recoger el item, para mostrar la puntuación obtenida.
    public GameObject floatPointsPrefab;

    private void Awake()
    {
        autoMovement = GetComponent<AutoMovement>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCatched)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<Mario>().CatchItem(type);
                //Destroy(gameObject);
                CatchItem();
            }
        }
    }

// Este método se llama cuando el collider del item entra en contacto con Mario
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
                //Destroy(gameObject);
                CatchItem();
            }
        }
    }

    public void WaitMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = false;
        }
    }

    // Este método se llama para iniciar el movimiento del item, ya sea mediante el componente AutoMovement o mediante una velocidad inicial.
    public void StartMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = true;
        }
        else
        {
            if (startVelocity != Vector2.zero)
            {
                GetComponent<Rigidbody2D>().velocity = startVelocity;
            }
        }
    }

    // Este método se llama cuando Mario golpea el bloque debajo del item, para cambiar la dirección del movimiento del item.
    public void HitBelowBlock()
    {
        if (autoMovement != null && autoMovement.enabled)
        {
            autoMovement.ChangeDirection();
        }
    }

    // Este método se llama cuando Mario recoge el item, para añadir la puntuación y destruir el item 
    void CatchItem()
    {
        ScoreManager.instance.AddScore(this.points);
        if (floatPointsPrefab != null)
        {
            GameObject floatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
            Points points = floatPoints.GetComponent<Points>();
            points.numPoints = this.points;
        }
        Destroy(gameObject);
    }
}
