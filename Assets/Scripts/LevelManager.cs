using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    // public HUD hud;
    // int coins;
    public static LevelManager instance;
    public int time;
    public float timer;

    Mario mario;

    // Referencias a los puntos de inicio y checkpoint del nivel
    public Transform spawnPoint;
    public Transform checkPoint;

    public bool hasLevelStart;

    public CameraMove cameraMove;
    public LevelMusic backGroundMusic;

    public bool levelFinished;
    public bool levelPaused;

    public bool countPoints;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Al inicar el nivel, se inicializan las variables y se reproduce la musica de fondo del nivel.
    void Start()
    {
        AudioManager.instance.PlayLevelStageMusic(backGroundMusic);

        timer = time;
        GameManager.instance.hud.UpdateTime(timer);

        mario = FindObjectOfType<Mario>();
        cameraMove = Camera.main.GetComponent<CameraMove>();
        GameManager.instance.LevelLoaded();
    }

    // Si el nivel no ha terminado y no esta pausado, se actualiza el temporizador. (Junto con la velocidad de la musica)
    void Update()
    {
        if (!levelFinished && !levelPaused)
        {
            timer -= Time.deltaTime;

            if (timer <= 100)
            {
                AudioManager.instance.SpeedMusic();
            }
            
            if (timer <= 0)
            {
                GameManager.instance.RunOutOfTime();
                timer = 0;
            }
            GameManager.instance.hud.UpdateTime(timer);
        }
    }

    // Cuando se termina el nivel, se inicia la rutina para contar los puntos restantes y se marca el nivel como terminado.
    public void FinishLevel()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoint());
    }

    // Corutina que se ejecuta al finalizar el nivel, cuenta los puntos restantes y actualiza la UI. (Hasta no haber calculado todos los puntos, no continuara el juego)
    IEnumerator SecondsToPoint()
    {
        yield return new WaitForSeconds(1f);

        int timeLeft = Mathf.RoundToInt(timer);

        while (timeLeft > 0)
        {
            timeLeft--;
            //hud.UpdateTime(timeLeft);
            GameManager.instance.hud.UpdateTime(timeLeft);
            ScoreManager.instance.AddScore(50);
            AudioManager.instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
        countPoints = true;
    }
}
