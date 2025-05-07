using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
         float time = 0;
         float duration = 0.25f;
         Vector2 startPosition = transform.position;
         Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 3f);

         while (time < duration)
         {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
         }
         transform.position = targetPosition;
         time = 0;
         while (time < duration)
         {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
         }
         transform.position = startPosition;
         Destroy(gameObject);
    }
}
