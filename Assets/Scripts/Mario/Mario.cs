using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Mario : MonoBehaviour
{
    //Cada uno representa un estado de Mario, y se empieza como Mario normal (Default).
    public enum State {Default = 0, Super = 1, Fire = 2}
    State currentState = State.Default;

    // Este será el objeto que contiene la hitbox de pisar
    public GameObject stompBox;
    public Move mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2d;

    public GameObject fireballPrefab;
    public Transform shootPos;

    // Detecta si Mario esta en modo invencible (Modo Estrella)
    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;

    // Detecta si Mario esta herido
    // y cuanto tiempo dura el estado de herido. (Es invencible durante este tiempo)
    public bool isHurt;
    public float hurtTime;
    float hurtTimer;
    public bool isCrouched;

    // Detecta si Mario esta muerto
    public bool isDead;
    
    // Creamos una instancia estática de Mario para que pueda ser accedida desde otros scripts.
    public static Mario instance;

    //public HUD hud;

    // Metodo Awake se ejecuta al iniciar el juego, y se encarga de inicializar la instancia de Mario. No permite que haya más de una instancia de Mario en la escena.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            mover = GetComponent<Move>();
            colisiones = GetComponent<Colisiones>();
            animaciones = GetComponent<Animaciones>();
            rb2d = GetComponent<Rigidbody2D>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
            //Se encarga de activar o desactivar la hitbox de pisar si esta cayendo
            if (rb2d.velocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                if (transform.parent == null)
                {
                    stompBox.SetActive(false);
                }
            }

        // Antigua forma de recibir input, ahora se usa InputTranslator
            // if (Input.GetKey(KeyCode.DownArrow))
            // {
            //     if (colisiones.isGrounded)
            //     {
            //         isCrouched = true;
            //     }
            // }

            // if (Input.GetKeyDown(KeyCode.Z))
            // {
            //     Shoot();
            // }

        //Nueva forma de recibir input, usando el script InputTranslator
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput < -0.5f && colisiones.isGrounded)
            {
                isCrouched = true;
            }

            if (InputTranslator.Fire)
            {
                Shoot();
            }

        // Si Mario es invencible (Estrella), se reduce el tiempo de invencibilidad y se detiene la musica cuando el tiempo es menor a 2 segundos.
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 2f)
                {
                    AudioManager.instance.StopMusicStar(true);
                }
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }

        // Si Mario esta herido, se reduce el tiempo de herido y se termina el estado de herido cuando el tiempo es menor o igual a 0.
            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }

        animaciones.Crouch(isCrouched);


    // CheatCodes para probar el juego y las transformaciones de Mario.
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            animaciones.PowerUp();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }

    // Logica cuando recibe daño Mario, dependiendo del estado en el que se encuentre.
    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead();
            }
            else
            {
                AudioManager.instance.PlayPowerDown();
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }

    }

    // Logica para entrar en Invulnerable despues de haber recibido daño.
    void StartHurt()
    {
        isHurt= true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);
    }

    // Logica para salir de Invulnerable despues de haber recibido daño.
    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);
        
    }

    // Logica para morir Mario, se detiene la musica, se reproduce el sonido de muerte y se cambia el estado a muerto.
    public void Dead()
    {
        if (!isDead)
        {
            AudioManager.instance.PlayDie();
            isDead = true;
            colisiones.Dead();
            mover.Dead();
            animaciones.Dead();
            GameManager.instance.LoseLife();
        }
    }

    // Logica para resucitar Mario, se reinician las animaciones, se cambia el estado a Default y se resetean las colisiones y el movimiento.
    public void Respawn(Vector2 pos)
    {
        if (isDead)
        {
            animaciones.Reset();
            currentState = State.Default;
        }
        isDead = false;
        colisiones.Respawn();
        mover.Respawn();
        transform.position = pos;
    }

    // Cambia el estado de Mario a uno nuevo, se actualizan las animaciones y se reinicia el tiempo.
    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;
    }

    // Logica al tocar un item, dependiendo del tipo de item se cambia el estado de Mario o se ejecuta una accion.
    public void CatchItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.MagicMushroom:
                AudioManager.instance.PlayPowerUp();
                if (currentState == State.Default)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2d.velocity = Vector2.zero;
                }
                break;

            case ItemType.FireFlower:
                AudioManager.instance.PlayPowerUp();
                if (currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2d.velocity = Vector2.zero;
                }
                break;

            case ItemType.Coin:
                AudioManager.instance.PlayCoin();
                GameManager.instance.AddCoin();
                break;

            case ItemType.Life:
                GameManager.instance.NewLife();
                AudioManager.instance.Play1UP();
                break;

            case ItemType.Star:
                AudioManager.instance.PlayPowerUp();
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt();
                AudioManager.instance.MusicStar();
                break;

            default:
                break;
        }
    }

    // Logica para disparar una bola de fuego, se instancia el prefab de la bola de fuego y se le asigna la direccion. (No se podrá disparar si Mario está agachado)
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            AudioManager.instance.PlayShoot();
            GameObject newFireball = Instantiate(fireballPrefab, shootPos.position, Quaternion.identity);
            newFireball.GetComponent<Fireball>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }

    // Devuelve el estado actual de Mario.
    public bool IsBig()
    {
        return currentState != State.Default;
    }

    // Logica para cuando Mario toca la bandera al final del nivel, se detiene la musica de la estrella, se reinicia el temporizador de invencibilidad, se reproduce el sonido de la bandera y se baja la bandera.
    public void Goal()
    {
        AudioManager.instance.StopMusicStar(false);
        invincibleTimer = 0;
        AudioManager.instance.PlayFlagPole();
        mover.DownFlagPole();
        //levelFinished = true;
        LevelManager.instance.FinishLevel();
    }
}
