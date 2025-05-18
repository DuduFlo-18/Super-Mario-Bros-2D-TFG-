using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosCamera : MonoBehaviour
{
    public Transform startPos;
    void Start()
    {
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float camPos = startPos.position.x + camWidth;
        transform.position = new Vector3(camPos, transform.position.y, transform.position.z);
    }
}
