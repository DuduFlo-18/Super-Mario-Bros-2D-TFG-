using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Librerias extra usadas
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StartMenu : MonoBehaviour
{
    public TextMeshProUGUI topScore;
    public Vector2 marioSpawn;
    public GameObject buttonDefault;
    public GameObject buttonContinue;

    void Start()
    {
        StartCoroutine(GetHighscoreFromServer()); 

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

    //Corrutina para solicitar la máxima puntuación al servidor y actualizar el UI con formato
    IEnumerator GetHighscoreFromServer()
    {
        Debug.Log("Intentando conectar a la API de puntuaciones...");
        UnityWebRequest request = UnityWebRequest.Get("https://mario-api-576905321923.europe-west1.run.app/highscore");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error obteniendo puntuación: " + request.error);
        }
        else
        {
            // Suponemos que la respuesta es JSON: { "highscore": <int> }
            string json = request.downloadHandler.text;
            HighscoreResponse data = JsonUtility.FromJson<HighscoreResponse>(json);
            if (data != null)
            {
                int maxScore = data.highscore;
                topScore.text = "TOP - " + maxScore.ToString("D6");
                // Actualizar el maxScore en ScoreManager (opcional, para referencia local)
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.maxScore = maxScore;
                }
            }
        }
    }
    // Botones del menú principal
   public void ButtonNewGame()
    {
        GameManager.instance.StartGame();
    }

    public void ButtonContinueGame()
    {
        GameManager.instance.ContinueGame();
    }


    // Clase auxiliar para deserializar la respuesta JSON
    [System.Serializable]
    private class HighscoreResponse {
        public int highscore;
    }
}
