using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de gestionar el comportamiento de la bola de fuego lanzada por Mario, incluyendo su movimiento, colisiones y explosion al chocar.
public class Fireball : MonoBehaviour
{
    //Valores que se pueden ajustar desde el inspector de Unity 
    public float direction;
    public float speed;
    public float bounceForce;

    public GameObject explosionPrefab;
    Rigidbody2D rb2d;

    bool colision;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    // Hace que la bola de fuego se mueva en la direccion indicada al lanzarla.
    void Start()
    {
        speed *= direction;
        rb2d.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
    }

    // Si la bola de fuego colisiona con un enemigo o con el suelo, se ejecuta la logica de colision. (Enemigo = recibe daño, Suelo = rebota, Pared = explota)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.HitFireBall();
            //Destroy(gameObject);
            Explode(collision.GetContact(0).point);
        }

        else
        {
            Vector2 sidepoint = collision.GetContact(0).normal;
            if (Mathf.Abs(sidepoint.x) > 0.01f) //Colision lateral = Destruir
            {
                Explode(collision.GetContact(0).point);
            }
            else if (sidepoint.y > 0) //Colision con el suelo = Rebotar
            {
                rb2d.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            else if (sidepoint.y < 0) // Colisiona con algo por arriba = Rebotar
            {
                rb2d.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
            }
            else
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);
            }
        }
    }

    // Si la bola de fuego sigue colisionando con el mismo objeto, explota.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (colision)
        {
            colision = false;
        }
        else
        {
            Explode(collision.GetContact(0).point);
        }
    }

    // Crea una explosion en el punto de colision y destruye la bola de fuego.
    void Explode(Vector2 point)
    {
        AudioManager.instance.PlayBump();
        Instantiate(explosionPrefab, point, Quaternion.identity);
        Destroy(gameObject);
    }
}
