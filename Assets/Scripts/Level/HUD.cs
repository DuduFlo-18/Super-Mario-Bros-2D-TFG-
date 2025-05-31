using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usamos TMPro para el texto, ya que es más versátil que el Text de Unity.
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

// Funcionalidad del HUD (Head-Up Display) del juego, que muestra la puntuación, monedas, tiempo y nivel actual.
public class HUD : MonoBehaviour
{
    public TextMeshProUGUI score;
    //int coins;
    public TextMeshProUGUI numCoins;

    public TextMeshProUGUI time;

    public TextMeshProUGUI worldLevel;

    // Le añadimos formato D6 para que siempre muestre 6 dígitos, rellenando con ceros a la izquierda si es necesario.
    void Update()
    {
        score.text = ScoreManager.instance.score.ToString("D6");
    }

    // Actualiza la puntuación del HUD.
    public void UpdateCoins(int totalcoins)
    {
        numCoins.text = "x" + totalcoins.ToString("D2");
    }

    // Actualiza el tiempo restante en el HUD.
    public void UpdateTime(float timeLeft)
    {
        int timeLeftInt = Mathf.RoundToInt(timeLeft);
        time.text = timeLeftInt.ToString("D3");
    }

    // Actualiza el nivel actual del HUD, mostrando el mundo y el nivel en formato "Mundo-Nivel".
    public void UpdateWorld(int world, int level)
    {
        worldLevel.text = world + "-" + level;
    }
}
