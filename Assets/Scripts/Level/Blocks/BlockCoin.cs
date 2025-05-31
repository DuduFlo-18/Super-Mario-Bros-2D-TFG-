using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de dar funcionalidad a los bloques de monedas en el juego.
public class BlockCoin : MonoBehaviour
{
    //Añadimos un prefab de puntos para que se muestren los puntos al conseguir monedas en los bloques
    public GameObject pointsPrefab;

    // Al golpear el bloque, se añade una moneda al GameManager, se reproduce un sonido de moneda.
    void Start()
    {
        GameManager.instance.AddCoin();
        AudioManager.instance.PlayCoin();
        ScoreManager.instance.AddScore(200);

        Vector2 positionPoints = new Vector2(transform.position.x, transform.position.y + 1f);

        GameObject newFloatPoint = Instantiate(pointsPrefab, positionPoints, Quaternion.identity);
        Points floatPoints = newFloatPoint.GetComponent<Points>();
        floatPoints.numPoints = 200;
        StartCoroutine(Animation());
    }

    // Metodo para animar el bloque al ser golpeado
    IEnumerator Animation()
    {
        float time = 0;
        float duration = 0.25f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 3f);

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        Destroy(gameObject);
    }
}
