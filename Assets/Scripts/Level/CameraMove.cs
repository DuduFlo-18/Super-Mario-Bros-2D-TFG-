using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    //Poder pasarle el transform del jugador
    public Transform target;
    public float follow = 2.5f;
    public float minPosX;
    public float maxPosX;

    float camWidth;
    float lastPos;

    public Transform leftLimit;
    public Transform rightLimit;

    public Transform colLeftLimit;
    public Transform colRightLimit;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minPosX = leftLimit.position.x + camWidth;
        maxPosX = rightLimit.position.x - camWidth;
        lastPos = minPosX;

        colLeftLimit.position = new Vector2(transform.position.x - camWidth - 0.5f, colLeftLimit.position.y);
        colRightLimit.position = new Vector2(transform.position.x + camWidth + 0.5f, colRightLimit.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && canMove)
        {
            float newPosX = target.position.x + follow;
            //Limitar la camara para que no se salga de los limites
            newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        }
    }

    public void StartFollow(Transform newTarget)
    {
        target = newTarget;
        float newPosX = target.position.x + follow;
        newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
        canMove = true;
    }

    public void UpdateRightLimit(float newLimit)
    {
        maxPosX = newLimit - camWidth;
        colRightLimit.position = new Vector2(maxPosX + 0.5f, colRightLimit.position.y);
    }
}
