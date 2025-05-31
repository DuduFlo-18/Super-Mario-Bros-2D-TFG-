using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de detectar la colision con la colision del Castillo y teletransportar a Mario al siguiente nivel.
public class CastleLevel : MonoBehaviour
{
    //Detecta si Mario ha entrado en el castillo (Final del nivel)
    bool marioInCastle;

    // Al terminar el nivel, se llama a GameManager para pasar al siguiente nivel.
    void Update()
    {
        if (marioInCastle && LevelManager.instance.countPoints)
        {
            GameManager.instance.NextLevel();
            marioInCastle = false;
        }
    }

    // Detecta la colision con Mario y lo teletransporta fuera de la escena.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if (mario != null)
        {
            mario.transform.position = new Vector3(1000, 1000, 1000);
            marioInCastle = true;
        }
    }
}
