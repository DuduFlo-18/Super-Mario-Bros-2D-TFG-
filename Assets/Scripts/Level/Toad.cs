using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


// Este script se encarga de gestionar la interacción con el personaje Toad al final del nivel, mostrando mensajes y permitiendo regresar al menu principal.
public class Toad : MonoBehaviour
{

    // Textos que se mostrarán al jugador al completar el nivel.
    public GameObject ToadText;
    public GameObject ToadText2;
    public GameObject ToadText3;
    public GameObject ToadText4;
    public GameObject ToadText5;

    // Al tocar al Toad, se detiene el movimiento del jugador y se muestran los textos de finalización del nivel en orden secuencial.
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

    // Corutina que muestra los textos de finalización del nivel uno por uno, con pausas entre cada uno, y espera a que el jugador presione una tecla para finalizar y regresar al menú principal.
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

        yield return new WaitForSeconds(1f);
        ToadText5.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.JoystickButton0) || InputTranslator.customHorizontal == -1f);
         if (Mario.instance != null)
        {
            Mario.instance.SetSmall(); // 🔁 Aseguramos que Mario sea pequeño ANTES de volver al menú
        }
        ScoreManager.instance.GameOver();
        SceneManager.LoadScene("StartMenu");
        ScoreManager.instance.NewGame();

       
    }
}
