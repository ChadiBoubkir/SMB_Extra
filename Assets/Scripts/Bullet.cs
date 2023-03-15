using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject mario;
    private Player player;

    private new Rigidbody2D rigidbody;

    private float lifeDuration = 6f;

    public float speed = 13f;

    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("Player");
        player = mario.GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().flipX = true;
        rigidbody.velocity = transform.right * -speed;
    }

    void Update() {
        lifeDuration -= Time.deltaTime;
        if (lifeDuration <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            if (!player.starpower) {
                player.Hit();
            }
        }
        Destroy(gameObject);
    }
}
