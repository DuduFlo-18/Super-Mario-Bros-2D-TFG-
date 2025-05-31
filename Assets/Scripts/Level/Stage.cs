using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Este script se encarga de gestionar las características del nivel, como la posición de entrada, la dirección de entrada, la cámara de seguimiento y la música de fondo dependiendo del lugar.
// Este script se relaciona con el script StageConnection.cs
public class Stage : MonoBehaviour
{
    public Transform enterPoint;
    public ConnectDirection enterDirection;

    public CameraMove cameraFollow;
    public bool cameraMove;

    public LevelMusic backgroundMusic;
    public Color backgroundColor;

    // Start is called before the first frame update
    void StartStage()
    {
        Mario.instance.mover.ResetMove();
        LevelManager.instance.levelPaused = false;
        if (cameraMove)
        {
            cameraFollow.StartFollow(Mario.instance.transform);
        }
    }

    // Lógica al entrar al nivel
    public void EnterStage()
    {
        // Características del nivel
        AudioManager.instance.PlayLevelStageMusic(backgroundMusic);
        Camera.main.backgroundColor = backgroundColor;
        Mario.instance.transform.position = enterPoint.position;

        // Si el nivel tiene una cámara de seguimiento, se mueve la cámara a la posición del Mario.
        cameraFollow.transform.position = new Vector3(transform.position.x, transform.position.y, cameraFollow.transform.position.z);

        // Si el nivel tiene una dirección de entrada, se mueve al Mario a la posición de entrada y se inicia el movimiento. (Solo realice hacia abajo y arriba pero deje codigo realizado para las otras direcciones)
        switch (enterDirection)
        {
            case ConnectDirection.Up:
                //StartStage();
                StartCoroutine(StartStageUp());
                break;
            case ConnectDirection.Down:
                //StartStage();
                StartCoroutine(StartStageDown());
                break;
            case ConnectDirection.Left:
                StartStage();
                break;
            case ConnectDirection.Right:
                StartStage();
                break;
        }
    }

    // Animacion de bajada de Mario (Usada al meterte dentro de la tubería).
    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }

    // Animacion de subida de Mario (Usada al salir de la tubería).
    IEnumerator StartStageUp()
    {
        float tamañoMario = Mario.instance.GetComponent<SpriteRenderer>().bounds.size.y;
        Mario.instance.transform.position = enterPoint.position + Vector3.down * tamañoMario;
        Mario.instance.mover.AutomoveConnection(enterDirection);
        while (!Mario.instance.mover.moveConnectionComplete)
        {
            yield return null;
        }
        StartStage();
    }
}
