using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    private int maxBounces = 3;

    private float bounceHeight = 9f;

    public float speed = 7f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //GetComponent<SpriteRenderer>().flipX = true;
        rigidbody.velocity = transform.right * /*-*/speed;
    }

    void Update() {
        if (maxBounces <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Goomba" || collider.gameObject.tag == "Koopa" || collider.gameObject.tag == "Boss") {
            Destroy(gameObject, 0.0001f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "DestroyFireball") {
            if (transform.DotTest(collision.transform, Vector2.down)) {
                maxBounces -= 1;
                rigidbody.velocity += new Vector2(0f, bounceHeight); 
            }else {
                Destroy(gameObject);
            }
        }
    }
}
