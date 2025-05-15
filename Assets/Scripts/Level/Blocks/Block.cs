using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    //public GameObject pointsPrefab;

    //List<GameObject> enemiesOnBlock = new List<GameObject>();
    public LayerMask onBlockLayers;
    BoxCollider2D boxCollider2D;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

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

    private void OnDrawGizmos()
    {
        if (boxCollider2D != null)
        {
            Gizmos.DrawWireCube(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f);
        } 
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         Vector2 sideContact = collision.contacts[0].normal;
    //         Vector2  topSide = new Vector2(0f, -1f);

    //         if (sideContact == topSide)
    //         {
    //             if (!enemiesOnBlock.Contains(collision.gameObject))
    //             {
    //                 enemiesOnBlock.Add(collision.gameObject);
    //             }
    //         }
    //     }
    // }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (!bouncing)
    //     {
    //         if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //         {
    //         if (enemiesOnBlock.Contains(collision.gameObject))
    //             {
    //                 enemiesOnBlock.Remove(collision.gameObject);
    //             }
    //         }
    //     }
    // }

    //private void OnTriggerEnter2D(Collider2D collision)
    public void HeadCollision(bool marioBig)
    {
        // if (collision.CompareTag("HeadMario"))
        // {
        //     //Obtengo el componente de Mario cortar el salto cuando golpea a un bloque
        //     collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (isBreakable)
            {
                if (marioBig)
                {
                    Debug.Log("Bloque golpeado");
                    //Si el bloque es rompible y Mario es grande, se rompe
                    Break();
                }
                else
                {
                    Debug.Log("Bloque Chocado");
                    //Si el bloque es rompible y Mario es pequeño, no se rompe
                Bounce();
                }
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
                        Bounce();
                        isEmpty = true;
                    }
                }
            }
            if (!isEmpty)
            {
                OntheBlock();
            }
        //     if (!isEmpty)
        //     {
        //         foreach(GameObject enemyOnBlock in enemiesOnBlock)
        //         {
        //             enemyOnBlock.GetComponent<Enemy>().HitBelowBlock();
        //         }
        //     }
           // }
            
            // Debug.Log("Q golpe illo");
            // Bounce();
            //Break();
    }

    void Bounce()
    {
        if (!bouncing)
        {
            StartCoroutine(BounceAnimation());
        }
    }

    IEnumerator BounceAnimation()
        {
            AudioManager.instance.PlayBump();
            bouncing  = true;
            float time = 0;
            float duration = 0.1f;

            Vector2 startPosition = transform.position;
            Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

            while(time < duration)
            {
                transform.position = Vector2.Lerp(startPosition, targetPosition, time/duration);
                time+= Time.deltaTime;
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
        
        void Break()
        {
            AudioManager.instance.PlayBreak();
            ScoreManager.instance.AddScore(50);
            GameObject brickPiece;
            //Arriba-Derecha
            brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
            brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, 12f);

            //Arriba-Izquierda
            brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0,0,90)));
            brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, 12f);

            //Abajo-Derecha
            brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0,0,-90)));
            brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(6f, -8f);

            //Abajo-Izquierda
            brickPiece = Instantiate(brickPrefab, transform.position, Quaternion.Euler(new Vector3(0,0,180)));
            brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-6f, -8);

            Destroy(gameObject);
        }

        IEnumerator ShowItem()
        {
            AudioManager.instance.PlayPowerAppear();
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

        //     AutoMovement autoMovement = newItem.GetComponent<AutoMovement>();
        // //Si tiene movimiento automatico se le detiene ese movimiento
        //     if (autoMovement != null)
        //     {
        //         autoMovement.enabled = false;
        //     }

            Item item = newItem.GetComponent<Item>();
            item.WaitMove();
            float time = 0;
            float duration = 1f;
            Vector2 startPosition = newItem.transform.position;
            Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.5f;

            while (time < duration)
            {
                newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time/duration);
                time+= Time.deltaTime;
                yield return null;
            }
            newItem.transform.position = targetPosition;
            // if (autoMovement != null)
            // {
            //     autoMovement.enabled = true;
            // }
            item.StartMove();
        }
}