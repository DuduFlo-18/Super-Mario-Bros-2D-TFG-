using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script permite conectar diferentes etapas del juego, permitiendo al jugador moverse entre ellas mediante flechas de dirección.
public enum ConnectDirection
{
    Up,
    Down,
    Left,
    Right
}
public class StageConnection : MonoBehaviour
{
    public ConnectDirection exitDirection;

    public CameraMove cameraFollow;
    bool connectionStarted;
    bool stayConnection;

    public Stage stage;

    bool inputInUse = false;

    // Si el jugador se encuentra en la conexión (entre zonas), se le permite iniciar la conexión al presionar las teclas de dirección correspondientes.
    private void Update()
    {

        //Prueba de entrada en tubería para Input de forma normal para teclado
        // if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && exitDirection == ConnectDirection.Down)
        // {
        //     if (stayConnection && !connectionStarted)
        //     {
        //         StartCoroutine(StartConnection());
        //     }
        // }
        // else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && exitDirection == ConnectDirection.Right)
        // {
        //     if (stayConnection && !connectionStarted)
        //     {
        //         StartCoroutine(StartConnection());
        //     }
        // }


        //Prueba de entrada en tubería para Input de Teclado, Mando y Táctil
        float vertical = InputTranslator.Vertical;
    float horizontal = InputTranslator.Horizontal;

    if (!connectionStarted && stayConnection)
    {
        if (!inputInUse)
        {
            if (vertical < -0.5f && exitDirection == ConnectDirection.Down)
            {
                StartCoroutine(StartConnection());
                inputInUse = true;
            }
            else if (horizontal > 0.5f && exitDirection == ConnectDirection.Right)
            {
                StartCoroutine(StartConnection());
                inputInUse = true;
            }
            else if (vertical > 0.5f && exitDirection == ConnectDirection.Up)
            {
                StartCoroutine(StartConnection());
                inputInUse = true;
            }
            else if (horizontal < -0.5f && exitDirection == ConnectDirection.Left)
            {
                StartCoroutine(StartConnection());
                inputInUse = true;
            }
        }

        // Reset inputInUse cuando no hay input
        if (Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f)
        {
            inputInUse = false;
        }
    }
    }

    // Inicia la conexión con la etapa correspondiente, moviendo al jugador a la posición de entrada de la nueva etapa y desactivando el movimiento de la cámara.
    IEnumerator StartConnection()
    {
        AudioManager.instance.PlayPipe();
        connectionStarted = true;
        LevelManager.instance.levelPaused = true;
        cameraFollow.canMove = false;
        Mario.instance.mover.AutomoveConnection(exitDirection);

        while(!Mario.instance.mover.moveConnectionComplete)
        {
            yield return null;
        }
        stage.EnterStage();
    }

    // Detecta cuando el jugador entra, permanece o sale de la zona de conexión.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stayConnection = true;
        }
    }

    // Detecta cuando el jugador sale de la zona de conexión, desactivando la posibilidad de iniciar la conexión a la nueva etapa.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stayConnection = false;
        }
    }
}
