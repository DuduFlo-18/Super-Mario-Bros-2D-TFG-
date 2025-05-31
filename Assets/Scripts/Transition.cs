using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Este script controla la pantalla de Carga entre niveles o al finalizar el juego.
public class Transition : MonoBehaviour
{
    public TextMeshProUGUI numLives;
    public TextMeshProUGUI world;
    public GameObject panelLevel;
    public GameObject panelGameOver;
    // Gestionamos la transición entre niveles y el estado del juego, mostrando información relevante como el número de vidas y el nivel actual.
    void Start()
    {
        GameManager.instance.HideTimer();
        if (GameManager.instance.isGameOver)
        {
            panelLevel.SetActive(false);
            panelGameOver.SetActive(true);
        }
        else
        {
            numLives.text = GameManager.instance.lives.ToString();

            world.text = GameManager.instance.currentWorld + "-" + GameManager.instance.currentLevel;
            panelLevel.SetActive(true);
            panelGameOver.SetActive(false);
        }
    }
}


