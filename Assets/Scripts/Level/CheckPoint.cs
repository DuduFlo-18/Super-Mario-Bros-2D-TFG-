using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de activar un checkpoint cuando el jugador lo toca.
public class CheckPoint : MonoBehaviour
{
    // Al tocar la Hitbox del checkpoint, se activa el checkpoint en el GameManager. Y al morir el jugador respawnea en el checkpoint.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Has tocado el checkpoint");
            GameManager.instance.checkpoint = true;
        }
    }
}
