using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public HUD hud;
    int coins;
    public Mario mario;
    public int lives;

    bool isRespawning;
    bool isGameOver;

    public bool checkpoint;

    public static GameManager instance;
    private void Awake()
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
        lives = 3;
        coins = 97;
        hud.UpdateCoins(coins);
    }

    public void AddCoin()
    {
        coins++;
        if (coins > 99)
        {
            coins = 0;
            NewLife();
        }

        hud.UpdateCoins(coins);
    }

    public void KillZone()
    {
        if (!isRespawning)
        {
            AudioManager.instance.PlayDie();
            LoseLife();
        }

    }

    public void RunOutOfTime()
    {
        mario.Dead();
    }

    public void LoseLife()
    {
        if (!isRespawning)
        {
            lives--;
            isRespawning = true;
            if (lives > 0)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                GameOver();
            }
        }
    }

    public void NewLife()
    {
        lives++;
        AudioManager.instance.Play1UP();
    }

    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.instance.NewGame();
        checkpoint = false;
    }

    void GameOver()
    {
        //isGameOver = true;
        Debug.Log("Game Over");
        isGameOver = true;
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        isRespawning = false;
        SceneManager.LoadScene(0);
    }

    public void LevelLoaded()
    {
        if (isGameOver)
        {
            NewGame();
        }

        if (checkpoint)
        {
            Mario.instance.Respawn(LevelManager.instance.checkPoint.position);
        }
        else
        {
            Mario.instance.Respawn(LevelManager.instance.spawnPoint.position);
        }

        LevelManager.instance.cameraMove.StartFollow(Mario.instance.transform);
    }
}
