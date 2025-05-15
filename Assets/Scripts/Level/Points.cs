using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Points : MonoBehaviour
{
    public PointsData[] pointsData;
    public int numPoints = 0;
    public float distance = 2f;
    public float speed = 2f;
    public bool destroy = true;

    float targetPos;


    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowPoints(numPoints);
        targetPos = transform.position.y + distance;
    }

    private void Update()
    {
        if (transform.position.y < targetPos)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (speed*Time.deltaTime));
        }
        else
        {
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
    void ShowPoints(int pointsSelected)
    {
        for (int i = 0; i < pointsSelected; i++)
        {
            if (pointsData[i].numPoints == pointsSelected)
            {
                spriteRenderer.sprite = pointsData[i].sprite;
                break;
            }
        }
    }
}

[Serializable]
public class PointsData
{
    public int numPoints;
    public Sprite sprite;
}
