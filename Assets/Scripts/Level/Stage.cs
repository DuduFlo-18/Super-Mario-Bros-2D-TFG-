using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public void EnterStage()
    {
        AudioManager.instance.PlayLevelStageMusic(backgroundMusic);
        Camera.main.backgroundColor = backgroundColor;
        Mario.instance.transform.position = enterPoint.position;
        cameraFollow.transform.position = new Vector3(transform.position.x, transform.position.y, cameraFollow.transform.position.z);

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

    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }

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
