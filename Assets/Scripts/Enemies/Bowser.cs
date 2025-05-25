using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : Enemy
{
    public int BowserHealth = 5;

    public bool isBowserDead;

    public float speed = 2f;
    public float minJumpTime = 1f;
    public float maxJumpTime = 5f;

    public float jumpForce = 6f;
    public float DistanceFromMarioToMove = 9f;

    float jumpTimer;
    float direction = -1;
    bool canMove = true;

    bool canShot;
    public GameObject firePrefab;
    public Transform shootPos;
    public float minShootTime = 1f;
    public float maxShootTime = 5f;
    float shotTimer;
    public float minDistanceShot = 50f;

    public bool collapseBridge = false;

    protected override void Start()
    {
        base.Start();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        shotTimer = Random.Range(minShootTime, maxShootTime);
        canMove = false;
        canShot = false;
    }

    protected override void Update()
    {
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

            if (!canShot && Mathf.Abs(Mario.instance.transform.position.x - transform.position.x) <= minDistanceShot)
            {
                canShot = true;
            }

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
