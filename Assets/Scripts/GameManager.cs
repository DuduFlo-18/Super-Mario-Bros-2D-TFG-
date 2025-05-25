using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public World[] worlds;
    public int currentWorld;
    public int currentLevel;
    public HUD hud;
    int coins;
    public Mario mario;
    public int lives;

    bool isRespawning;
    public bool isGameOver;

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
        // lives = 3;
        // coins = 97;
        //hud.UpdateCoins(coins);
        HideTimer();
        isGameOver = true;

        //Esta linea permitira guardar en que mundo y nivel estamos actualmente (Predeterminado 1)
        currentWorld = PlayerPrefs.GetInt("World", 1);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        
        Debug.Log("Progreso cargado: World = " + currentWorld + ", Level = " + currentLevel);
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.N))
    //     {
    //         StartGame();
    //     }
    //     if (Input.GetKeyDown(KeyCode.C))
    //     {
    //         ContinueGame();
    //     }
    // }

    

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

    public void StartGame()
    {
        currentLevel = 1;
        currentWorld = 1;
        LoadLevel();
    }

    public void ContinueGame()
    {
        LoadLevel();
    }


//Al perder se deben de comprobar los puntos para saber si ha habido record
    void GameOver()
    {
        Debug.Log("Game Over");
        ScoreManager.instance.GameOver();
        isGameOver = true;
        //currentLevel = 1;
        //currentWorld = 1;

        PlayerPrefs.SetInt("World", currentWorld);
        PlayerPrefs.SetInt("Level", currentLevel);
        StartCoroutine(Respawn());
    }


    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        isRespawning = false;
        //SceneManager.LoadScene(0);
        //LoadTransition();
        SceneManager.LoadScene("Transition");


        //Comprueba si se ha muerto o respawnea. Y carga el menu inicial o el respawn ingame.
        if (isGameOver)
        {
            AudioManager.instance.PlayGameover();
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("StartMenu");
        }
        else
        {
            yield return new WaitForSeconds(5f);
            LoadLevel();
        }
    }

    public void LevelLoaded()
    {
        hud.UpdateWorld(currentWorld, currentLevel);
        ShowTimer();
        if (isGameOver)
        {
            NewGame();
        }
        //Monedas a 0 en caso que sea game over
        hud.UpdateCoins(coins);

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
    public void GoToLevel(string level)
    {
        checkpoint = false;
        SceneManager.LoadScene(level);

    }

    public void GoToLevel(int world, int level)
    {
        checkpoint = false;
        currentLevel = level;
        currentWorld = world;
        hud.UpdateWorld(world, level);
        LoadTransition();
    }

    void LoadLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        string sceneName = worlds[worldIndex].levels[levelIndex].sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void NextLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        levelIndex++;
        if (levelIndex >= worlds[worldIndex].levels.Length)
        {
            levelIndex = 0;
            worldIndex++;
            if (worldIndex >= worlds.Length)
            {
                Debug.Log("Fin del juego");
                return;
            }
            else
            {
                levelIndex = 0;
            }
        }
        currentLevel = levelIndex + 1;
        currentWorld = worldIndex + 1;
        checkpoint = false;
        hud.UpdateWorld(currentWorld, currentLevel);

        PlayerPrefs.SetInt("World", currentWorld);
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.Save();

        Debug.Log("Progreso guardado: World = " + currentWorld + ", Level = " + currentLevel);

        LoadTransition();
    }

    void LoadTransition()
    {
        //Carga la escena de transicion, dependiendo de si es game over o no mostramos el panel de game over o el de level.
        SceneManager.LoadScene("Transition");
        //Temporizador para la escena.
        Invoke("LoadLevel", 5f);
    }

    public void HideTimer()
    {
        hud.time.enabled = false;
    }

    public void ShowTimer()
    {
        hud.time.enabled = true;
    }
}

[System.Serializable]
public struct World
{
    public int id;
    public Level[] levels;
}


[System.Serializable]
public struct Level
{
    public int id;
    public string sceneName;
}
