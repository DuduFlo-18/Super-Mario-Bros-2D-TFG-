using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float direction;
    public float speed;
    public float bounceForce;

    public GameObject explosionPrefab;
    Rigidbody2D rb2d;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        speed *= direction;
        rb2d.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0,0, speed*Time.deltaTime * -45);
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            //Debug.Log("SidePoint: " + sidepoint);

            if (sidepoint.x !=0) //Colision lateral = Destruir
            {
                //Destroy(gameObject);
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

    void Explode(Vector2 point)
    {
        AudioManager.instance.PlayBump();
        Instantiate(explosionPrefab, point, Quaternion.identity);
        Destroy(gameObject);
    }
}
