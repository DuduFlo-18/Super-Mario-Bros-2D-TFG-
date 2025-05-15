using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    public HUD hud;
    int coins;
    public static LevelManager instance;
    public int time;
    public float timer;

    Mario mario;

    public bool levelFinished;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        coins = 0;
        hud.UpdateCoins(coins);

        timer = time;
        hud.UpdateTime(timer);
        
        mario = FindObjectOfType<Mario>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelFinished)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                mario.Dead();
                timer = 0;
            }
            hud.UpdateTime(timer);
        }
    }

    public void AddCoins()
    {
        coins++;
        hud.UpdateCoins(coins);
    }

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
            hud.UpdateTime(timeLeft);
            ScoreManager.instance.AddScore(50);
            AudioManager.instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
