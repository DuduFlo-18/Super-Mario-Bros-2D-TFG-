using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Toad : MonoBehaviour
{
    public GameObject ToadText;
    public GameObject ToadText2;
    public GameObject ToadText3;
    public GameObject ToadText4;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Mario.instance.mover.StopMove();
            //PlayerPrefs.SetInt("GameCompleted", 1);    // Marca de juego completado
            //Reiniciamos el valor de niveles para que no permita realizar "Continue"
            PlayerPrefs.SetInt("World", 1);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.Save();
            StartCoroutine(ShowTexts());
        }
    }

    IEnumerator ShowTexts()
    {
        AudioManager.instance.PlayCastleCompleted();
        yield return new WaitForSeconds(1f);
        ToadText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ToadText2.SetActive(true);

        yield return new WaitForSeconds(1f);
        ToadText3.SetActive(true);
        ToadText4.SetActive(true);


        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.JoystickButton0));
        ScoreManager.instance.GameOver();
        SceneManager.LoadScene("StartMenu");
        ScoreManager.instance.NewGame();
    }
}
