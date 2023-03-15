using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venoomba : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Shoot shoot;

    private bool seen;
    public bool bossFight;

    private float shootingCooldown = 3f;

    private bool invincible = false;
    private float invincibleDuration = 5f;

    private float HP = 3;
    public bool alive = true;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        seen = false;
        bossFight = false;
        shootingCooldown = Random.Range(2f, 4f);
    }

    private void OnBecameVisible()
    {
        seen = true;
    }

    private void OnBecameInvisible()
    {
        seen = false;
    }

    private void FixedUpdate() {
        if (seen && alive) {
            bossFight = true;
        }else {
            bossFight = false;
        }

        if (shootingCooldown > 0f && rigidbody.DetectPlayer()) {
            shootingCooldown -= Time.deltaTime;
        }else if (shootingCooldown <= 0f && rigidbody.DetectPlayer()) {
            shootingCooldown = Random.Range(2f, 4f);
            shoot.ShootBullet();
        }

        /*if (rigidbody.DetectPlayer()) {
            Debug.Log("Player is within reach!");
        }else if (!rigidbody.DetectPlayer()) {
            Debug.Log("Player is too far!");
        }*/

        if (invincible) {
            invincibleDuration -= Time.deltaTime;
        }
        if (invincibleDuration <= 0f) {
            invincible = false;
            invincibleDuration = 5f;
        }else if (!invincible) {
            invincibleDuration = 5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (player.starpower)
            {
                Die();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                playerMovement.Launch(transform);
                if (!invincible) {
                    Hit();
                }
            }
            else
            {
                if (alive) {
                    player.Hit();
                }
            }
        }else if (collision.gameObject.CompareTag("Projectile")) {
            if (!invincible) {
                Burn();
            }
        }
    }

    private void Hit()
    {
        StartCoroutine(GetHit());
        if (HP > 1)
        {
            invincible = true;
            HP -= 1f;
        }else
        {
            Die();
        }
    }

    private void Burn()
    {
        StartCoroutine(GetHit());
        if (HP > 0.5)
        {
            invincible = true;
            HP -= 0.5f;
        }else
        {
            Die();
        }
    }

    private IEnumerator GetHit() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            spriteRenderer.color = Color.red;

            yield return null;
        }

        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        alive = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 5f);
    }
}
