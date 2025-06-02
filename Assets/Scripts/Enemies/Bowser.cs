using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script controla el comportamiento de Bowser, el jefe final del juego.
// Bowser se mueve hacia Mario, salta y dispara fuego. Si es golpeado por Mario o una bola de fuego, pierde salud hasta que muere.
public class Bowser : Enemy
{
    // Bowser tiene 5 puntos de salud. Cuando llega a 0.
    public int BowserHealth = 5;

    public bool isBowserDead;

    // Velocidad de movimiento de Bowser y fuerza del salto.
    public float speed = 2f;
    public float jumpForce = 6f;
    

    float jumpTimer;
    float direction = -1;
    bool canMove = true;

    bool canShot;
    public GameObject firePrefab;
    public Transform shootPos;

    // Tiempos de salto aleatorios entre un mínimo y un máximo.
    public float minJumpTime = 1f;
    public float maxJumpTime = 5f;

    // Tiempos de disparo aleatorios entre un mínimo y un máximo.
    public float minShootTime = 1f;
    public float maxShootTime = 5f;
    float shotTimer;

    // Distancia mínima para que Bowser empiece a disparar fuego.
    public float minDistanceShot = 50f;

    // Distancia mínima desde Mario para que Bowser empiece a moverse.
    public float DistanceFromMarioToMove = 9f;

    public bool collapseBridge = false;

    protected override void Start()
    {
        base.Start();
        // Inicializamos lo valores de Bowser asignando los tiempos de salto y disparo aleatorios.
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        shotTimer = Random.Range(minShootTime, maxShootTime);

        //Bowser no puede moverse ni disparar al inicio.
        canMove = false;
        canShot = false;
    }

    protected override void Update()
    {
        //En caso que Bowser esté muerto, no se ejecuta la lógica de movimiento ni disparo.
        if (isBowserDead) return;

        //Si el puente no se ha colapsado, Bowser se mueve y dispara (cuando cumple cierta distancia de Mario).
        if (!collapseBridge)
        {
            if (!canMove && Mathf.Abs(Mario.instance.transform.position.x - transform.position.x) <= DistanceFromMarioToMove)
            {
                canMove = true;
            }
            if (canMove)
            {
                if (transform.position.x >= (Mario.instance.transform.position.x + 2f) && direction == 1)
                {
                    direction = -1;
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (transform.position.x <= (Mario.instance.transform.position.x - 2f) && direction == -1)
                {
                    direction = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                rb2d.velocity = new Vector2(speed * direction, rb2d.velocity.y);

                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    Jump();
                }
            }

            // Si Bowser está a una distancia mínima de Mario, puede empezar a disparar fuego.
            if (!canShot && Mathf.Abs(Mario.instance.transform.position.x - transform.position.x) <= minDistanceShot)
            {
                canShot = true;
            }

            // Si Bowser puede disparar, se reduce el temporizador de disparo y se dispara cuando llega a 0.
            if (canShot)
            {
                shotTimer -= Time.deltaTime;
                if (shotTimer <= 0)
                {
                    Shoot();
                }
            }
        }
    }

    void Jump()
    {
        Vector2 force = new Vector2(0, jumpForce);
        rb2d.AddForce(force, ForceMode2D.Impulse);
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }

    //Logica para crear las llamaradas de Bowser, instanciando un nuevo objeto cada x tiempo
    void Shoot()
    {
        GameObject fire = Instantiate(firePrefab, shootPos.position, Quaternion.identity);
        fire.GetComponent<BowserFire>().fireDirection = direction;
        shotTimer = Random.Range(minShootTime, maxShootTime);
    }

    //Logica para que Bowser muera, se le da una animacion de muerte y se destruye el objeto
    public void FallBridge()
    {
        AudioManager.instance.PlayBowserFall();
        Dead();
    }

    //Si Mario pisa a Bowser, este recibe daño
    public override void Stomped(Transform player)
    {
        player.GetComponent<Mario>().Hit();
    }

    //Si bowser es golpeado por una bola de fuego, se muere
    public override void HitFireBall()
    {
        rb2d.velocity = Vector2.zero;
        BowserHealth--;
        if (BowserHealth <= 0)
        {
            AudioManager.instance.PlayBowserFall();
            canShot = false;
            FlipDie();
            isBowserDead = true;
        }
    }

    //Si bowser es golpeado por Mario Invencible, se muere
    public override void HitStarman()
    {
        Dead();
    }
}
