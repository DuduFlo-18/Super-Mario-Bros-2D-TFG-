using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int maxScore;

    public int score = 0;

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
        Debug.Log("📡 Iniciando carga de highscore desde API...");
        StartCoroutine(GetHighScoreFromServer());
    }

    public void NewGame()
    {
        score = 0;
    }

    //Guardo de manera local y persistente si se ha roto el Record de Puntos
    public void GameOver()
    {
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
    
        public void AddScore(int amount)
    {
        score += amount;
    }


    
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
