using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// Este script se encarga de dar funcionalidad a los bloques del juego, como los bloques rompibles, los bloques de monedas y los bloques de items.
public class Block : MonoBehaviour
{
    public bool isBreakable;
    public GameObject brickPrefab;

    public int numCoins;
    public GameObject coinBlockPrefab;
    public bool bouncing;
    public Sprite emptyBlock;
    bool isEmpty;

    public GameObject itemPrefab;
    public GameObject itemFlowerPrefab;

    //public GameObject pointsPrefab;

    //List<GameObject> enemiesOnBlock = new List<GameObject>();
    public LayerMask onBlockLayers;
    BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Metodo que se encarga de detectar si hay enemigos o items por encima del bloque. (Si es item debería de rebotar y cambiar dirección)
    void OntheBlock()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f, 0, onBlockLayers);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitBelowBlock();
            }
            else
            {
                Item item = collider.GetComponent<Item>();
                if (item != null)
                {
                    item.HitBelowBlock();
                }
            }
        }
    }

    // Esto es solo para poder visualizar el cubo de colision en el editor del Unity, no tiene efecto en el juego.
    private void OnDrawGizmos()
    {
        if (boxCollider2D != null)
        {
            Gizmos.DrawWireCube(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f);
        }
    }

    //Metodo que se llama cuando Mario golpea el bloque con la cabeza
    public void HeadCollision(bool marioBig)
    {
        if (isBreakable)
        {
            if (marioBig)
            {
                //Si el bloque es rompible y Mario es grande, se rompe
                Break();
            }
            else
            {
                //Si el bloque es rompible y Mario es pequeño, no se rompe
                Bounce();
            }
        }
        //Logica para bloques que no son rompibles
        else if (isEmpty)
        {
            //Si el bloque ya esta vacio, no hace nada
            AudioManager.instance.PlayBump();
        }
        else if (!isEmpty)
        {
            if (numCoins > 0)
            {
                if (!bouncing)
                {
                    Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                    numCoins--;
                    Bounce();
                    if (numCoins <= 0)
                    {
                        isEmpty = true;
                    }
                }
            }
            else if (itemPrefab != null)
            {
                if (!bouncing)
                {
                    StartCoroutine(ShowItem());
                    isEmpty = true;
                    Bounce();
                }
            }
        }
        if (!isEmpty)
        {
            OntheBlock();
        }
    }

    // Logica de rebote del bloque cuando este interactua con Mario
    void Bounce()
    {
        if (!bouncing)
        {
            StartCoroutine(BounceAnimation());
        }
    }

    // Logica de rebote del bloque cuando este interactua con Mario
    IEnumerator BounceAnimation()
    {
        AudioManager.instance.PlayBump();
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

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
        bouncing = false;
        if (isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }
            GetComponent<SpriteRenderer>().sprite = emptyBlock;
        }
    }

    //Metodo que se llama cuando el bloque se rompe, este crea los fragmentos del bloque y los lanza en diferentes direcciones.
    void Break()
    {
        AudioManager.instance.PlayBreak();
        ScoreManager.instance.AddScore(50);
        GameObject brickPiece;
        //Arriba-Derecha
        brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, 12f);

        //Arriba-Izquierda
        brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, 12f);

        //Abajo-Derecha
        brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, -90)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, -8f);

        //Abajo-Izquierda
        brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, -8);

        Destroy(gameObject);
    }

    //Metodo que se encarga de mostrar el item en el bloque, este item puede ser una flor o un item normal.
    IEnumerator ShowItem()
    {
        AudioManager.instance.PlayPowerAppear();
        GameObject newItem;
        if (itemFlowerPrefab != null && Mario.instance.IsBig())
        {
            newItem = Instantiate(itemFlowerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }

        Item item = newItem.GetComponent<Item>();
        item.WaitMove();
        float time = 0;
        float duration = 1f;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.5f;

        // Mueve el item hacia arriba desde la posicion del bloque
        while (time < duration)
        {
            newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        newItem.transform.position = targetPosition;
        item.StartMove();
    }
}