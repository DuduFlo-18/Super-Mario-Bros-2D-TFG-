using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Librerias extra usadas
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public TextMeshProUGUI topScore;
    public Vector2 marioSpawn;
    public GameObject buttonDefault;
    public GameObject buttonContinue;
    // Start is called before the first frame update
    void Start()
    {
        //Pregunte el record de puntos y los muestra en el Menu
        int score = PlayerPrefs.GetInt("Puntos");
        topScore.text = "TOP - " + score.ToString("D6");
        Mario.instance.Respawn(marioSpawn);

        EventSystem.current.SetSelectedGameObject(buttonDefault);

        int savedWorld = PlayerPrefs.GetInt("World", 1);
        int savedLevel = PlayerPrefs.GetInt("Level", 1);
        if (savedWorld == 1 && savedLevel == 1)
        {
            buttonContinue.GetComponent<Button>().interactable = false;
            //Esto lo dejara borroso si no hay posibilidad de Continuar la partida.
            buttonContinue.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void ButtonNewGame()
    {
        GameManager.instance.StartGame();
    }

    public void ButtonContinueGame()
    {
        GameManager.instance.ContinueGame();
    }
}
