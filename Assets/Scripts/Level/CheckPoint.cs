using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
//        Debug.Log("Algo entró: " + collision.gameObject.name); // Esto debe imprimirse
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Jugador tocó el checkpoint");
            GameManager.instance.checkpoint = true;
        }
        // if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     GameManager.instance.checkpoint = true;
        // }
    }
}
