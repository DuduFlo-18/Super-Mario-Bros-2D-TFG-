using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// El KilLZone es un área que mata al jugador al entrar en contacto con ella, en este proyecto la cree cuando te caes del mapa al vacío.
public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance.KillZone();
        }
    }
}
