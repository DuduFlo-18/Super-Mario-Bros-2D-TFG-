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

    void Start()
    {
        AudioManager.instance.PlayLevelStageMusic(backGroundMusic);
        // coins = 0;
        // hud.UpdateCoins(coins);

        timer = time;
        GameManager.instance.hud.UpdateTime(timer);
        //hud.UpdateTime(timer);

        mario = FindObjectOfType<Mario>();
        cameraMove = Camera.main.GetComponent<CameraMove>();
        GameManager.instance.LevelLoaded();
    }

    // public void StartLevel(int currentPoint)
    // {
    //     AudioManager.instance.PlayLevelStageMusic(backGroundMusic);
    // }

    // Update is called once per frame
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
                // mario.Dead();
                GameManager.instance.RunOutOfTime();
                timer = 0;
            }
            //hud.UpdateTime(timer);
            GameManager.instance.hud.UpdateTime(timer);
        }
    }

    // public void AddCoins()
    // {
    //     coins++;
    //     hud.UpdateCoins(coins);
    // }

    public void FinishLevel()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoint());
    }

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
