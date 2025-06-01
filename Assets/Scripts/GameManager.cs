using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Guardamos el estado del juego, como los mundos, niveles, HUD, Mario, vidas, monedas y si estamos en un respawn o game over.
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
    // Inicializamos el juego, ocultamos el temporizador.
    void Start()
    {
        HideTimer();
        isGameOver = true;

        //Esta linea permitira guardar en que mundo y nivel estamos actualmente (Predeterminado 1)
        currentWorld = PlayerPrefs.GetInt("World", 1);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        
        Debug.Log("Progreso cargado: World = " + currentWorld + ", Level = " + currentLevel);
    }

    // Logica del contador de monedas, añadiendo monedas y comprobando si se ha llegado a 100 para dar una vida extra.
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

    // Esto es una HitBox creada en el vacío y caidas del juego para matar a Mario.
    public void KillZone()
    {
        if (!isRespawning)
        {
            AudioManager.instance.PlayDie();
            LoseLife();
        }

    }

    // Logica para cuando se termina el tiempo del nivel, se mata a Mario.
    public void RunOutOfTime()
    {
        mario.Dead();
    }

    // Logica para perder una vida, comprobando si estamos en respawn o no, y si quedan vidas o no.
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

    // Logica para ganar una vida, se incrementa el contador de vidas y se reproduce el sonido de 1UP.
    public void NewLife()
    {
        lives++;
        AudioManager.instance.Play1UP();
    }

    // Logica para iniciar un nuevo juego, reiniciando las vidas, monedas y estado del juego.
    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.instance.NewGame();
        checkpoint = false;

        // Reset de Input para evitar movimientos no deseados al iniciar el juego.
        InputTranslator.customHorizontal = 0f;
        InputTranslator.customVertical = 0f;
        InputTranslator.customJump = false;
        InputTranslator.customFire = false;
        InputTranslator.customCrouch = false;

        if (Mario.instance != null)
        {
            Mario.instance.SetSmall();
            Mario.instance.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    // Logica para iniciar el juego, reiniciando el nivel y mundo actual, y cargando el primer nivel.
    public void StartGame()
    {
        NewGame();
        currentLevel = 1;
        currentWorld = 1;
        LoadLevel();
    }

    // Logica para continuar el juego, cargando el nivel actual guardado en PlayerPrefs.
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


    // Coroutine para el respawn de Mario, espera 3 segundos y luego carga la escena de transición.
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

    // Metodo que se llama cuando se carga un nivel, actualiza el HUD con el mundo y nivel actual, muestra el temporizador y respawnea a Mario.
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

    // Metodo para ir a un nivel especifico, reinicia el checkpoint y carga la escena del nivel.
    public void GoToLevel()
    {
        checkpoint = false;
        SceneManager.LoadScene("Level" + currentWorld + "_" + currentLevel);
    }
    public void GoToLevel(string level)
    {
        checkpoint = false;
        SceneManager.LoadScene(level);

    }

    // Metodo para ir a un nivel especifico con el mundo y nivel
    public void GoToLevel(int world, int level)
    {
        checkpoint = false;
        currentLevel = level;
        currentWorld = world;
        hud.UpdateWorld(world, level);
        LoadTransition();
    }

    // Metodo para cargar el nivel actual, obtiene el nombre de la escena del mundo y nivel actual y lo carga.
    void LoadLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        string sceneName = worlds[worldIndex].levels[levelIndex].sceneName;
        SceneManager.LoadScene(sceneName);
    }

    // Metodo para ir al siguiente nivel, incrementa el nivel y mundo actual, guarda el progreso en PlayerPrefs y carga la escena de transición.
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

    // Metodo para cargar la escena de transición, que muestra un panel de game over o de nivel dependiendo del estado del juego.
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

// Estructuras para guardar los mundos y niveles del juego.
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
