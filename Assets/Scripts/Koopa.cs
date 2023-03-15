using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;
    public EntityMovement movement;
    public SpriteRenderer spriteRenderer;
    private Player player;
    public EntityMovement entityMovement;
    public AnimatedSprite animatedSprite;

    private bool shelled;
    private bool pushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }else if (collision.gameObject.CompareTag("Projectile")) {
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                if (other.transform.DotTest(transform, Vector2.down))
                {
                    Player player = other.GetComponent<Player>();

                    if (player.starpower)
                    {
                        Hit();
                    }
                    else
                    {
                        pushed = false;
                        EnterShell();
                    }
                }
                else
                {
                    Player player = other.GetComponent<Player>();

                    if (player.starpower)
                    {
                        Hit();
                    }
                    else
                    {
                        player.Hit();
                    }
                }
            }
        }
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }else if (other.gameObject.CompareTag("Projectile")) {
            Hit();
        }
    }

    private void EnterShell()
    {
        shelled = true;
        entityMovement.enabled = false;
        animatedSprite.enabled = false;
        spriteRenderer.sprite = shellSprite;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        spriteRenderer.flipY = true;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }
}
