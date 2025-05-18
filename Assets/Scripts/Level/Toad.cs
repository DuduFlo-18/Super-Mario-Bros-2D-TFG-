using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
            StartCoroutine(ShowTexts());
        }
    }

    IEnumerator ShowTexts()
    {
        yield return new WaitForSeconds(1f);
        ToadText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ToadText2.SetActive(true);

        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play1UP();
        ToadText3.SetActive(true);
        ToadText4.SetActive(true);

    }
}
