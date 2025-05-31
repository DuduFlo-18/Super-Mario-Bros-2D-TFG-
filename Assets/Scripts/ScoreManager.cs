using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Este script gestiona la puntuación del juego, incluyendo la carga y envío de puntuaciones a un servidor remoto haciendo uso de una API llamada "Mario API".
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int maxScore;

    public int score = 0;

    // URL de la API donde se enviarán y recibirán las puntuaciones
    private string apiUrl = "https://mario-api-576905321923.europe-west1.run.app/highscore";

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Evitamos que haya más de una instancia de ScoreManager
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Eliminamos la lectura de PlayerPrefs de la puntuación máxima:
        //maxScore = PlayerPrefs.GetInt("Puntos", 0);
        score = 0;
        maxScore = 0;
        Debug.Log("Iniciando carga de highscore desde API...");
        StartCoroutine(GetHighScoreFromServer());
    }

    // Reiniciamos la puntuación al iniciar un nuevo juego
    public void NewGame()
    {
        score = 0;
    }


    // Variable para evitar enviar la puntuación más de una vez
    private bool scoreSent = false;

    //Guardo de manera local y persistente si se ha roto el Record de Puntos
    public void GameOver()
    {
        if (scoreSent)
        {
            Debug.LogWarning("La puntuación ya fue enviada, se ignora esta llamada duplicada.");
            return;
        }
        scoreSent = true;

        // Enviamos la puntuación final al servidor
        StartCoroutine(SendScoreToServer(score));
        if (score > maxScore)
        {
            Debug.Log("Nuevo record de puntos: " + score);
            maxScore = score;
            //PlayerPrefs.SetInt("Puntos", maxScore);
        }
        else
        {
            Debug.Log("No se ha superado el record de puntos. Puntuación actual: " + score);
        }
    }

    // Método para añadir puntos a la puntuación actual
    public void AddScore(int amount)
    {
        score += amount;
    }


    // Coroutine para obtener el highscore desde el servidor
    private IEnumerator GetHighScoreFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Respuesta recibida: " + json);

            ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(json);
            maxScore = response.highscore;
            Debug.Log("Highscore cargado: " + maxScore);
        }
        else
        {
            Debug.LogError("Error al cargar highscore: " + request.error);
        }
    }

    // Coroutine para enviar la puntuación final al servidor
    private IEnumerator SendScoreToServer(int finalScore)
    {
        ScoreData data = new ScoreData { score = finalScore };
        string json = JsonUtility.ToJson(data);
        Debug.Log("Enviando score a la API: " + json);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score enviado con éxito.");
        }
        else
        {
            Debug.LogError("Error al enviar el score: " + request.error);
        }
    }

    // Clases para manejar la estructura de datos de la puntuación
    [System.Serializable]
    private class ScoreData
    {
        public int score;
    }

    [System.Serializable]
    private class ScoreResponse
    {
        public int highscore;
    }

}
