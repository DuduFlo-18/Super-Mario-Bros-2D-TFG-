using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && exitDirection == ConnectDirection.Down)
        {
            if (stayConnection && !connectionStarted)
            {
                StartCoroutine(StartConnection());
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && exitDirection == ConnectDirection.Right)
        {
            if (stayConnection && !connectionStarted)
            {
                StartCoroutine(StartConnection());
            }
        }
    }

    IEnumerator StartConnection()
    {
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stayConnection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stayConnection = false;
        }
    }
}
