using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usamos 
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI score;
    //int coins;
    public TextMeshProUGUI numCoins;

    public TextMeshProUGUI time;

    public TextMeshProUGUI worldLevel;

    // Start is called before the first frame update
    // void Start()
    // {
    //     coins = 0;
    //     numCoins.text = "x" + coins.ToString("D2");
    // }

    // Update is called once per frame
    void Update()
    {
        score.text = ScoreManager.instance.score.ToString("D6");
    }

    // public void UpdateCoins()
    // {
    //     coins ++;
    //     numCoins.text = "x" + coins.ToString("D2");
    // }

    public void UpdateCoins(int totalcoins)
    {
        numCoins.text = "x" + totalcoins.ToString("D2");
    }

    public void UpdateTime(float timeLeft)
    {
        int timeLeftInt = Mathf.RoundToInt(timeLeft);
        time.text = timeLeftInt.ToString("D3");
    }

    public void UpdateWorld(int world, int level)
    {
        worldLevel.text = world + "-" + level;
    }
}
