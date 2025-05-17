using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public TextMeshProUGUI numLives;
    public TextMeshProUGUI world;
    public GameObject panelLevel;
    public GameObject panelGameOver;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.HideTimer();
        if (GameManager.instance.isGameOver)
        {
            panelLevel.SetActive(false);
            panelGameOver.SetActive(true);
        }
        else
        {
            numLives.text = GameManager.instance.lives.ToString();

            world.text = GameManager.instance.currentWorld + "-" + GameManager.instance.currentLevel;
            panelLevel.SetActive(true);
            panelGameOver.SetActive(false);
        }
    }
}


