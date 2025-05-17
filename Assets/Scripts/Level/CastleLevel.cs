using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevel : MonoBehaviour
{
    // Nombre de la escena a cargar (Se añade en el inspector)
    //public string levelName;
    // public int nextWorld;
    // public int nextLevel;

    //Detecta si mario ha entrado en el castillo
    bool marioInCastle;

    void Update()
    {
        if (marioInCastle && LevelManager.instance.countPoints)
        {
            //GameManager.instance.GoToLevel(levelName);
            //GameManager.instance.GoToLevel(nextWorld, nextLevel);
            GameManager.instance.NextLevel();
            marioInCastle = false;
        }
    }

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
