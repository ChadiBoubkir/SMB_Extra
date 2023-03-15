using UnityEngine;

public class VenoombaMovement : MonoBehaviour
{
    private GameObject mario;
    private Venoomba venoomba;

    private bool activated = false;
    private bool visible = false;

    public float speed = 3.5f;
    public Vector2 direction = Vector2.zero;
    private float jumpCooldown;

    public float maxJumpHeight = 6f;
    public float maxJumpTime = 4f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f) + 6f;
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2) - 17f;
    public bool grounded { get; private set; }

    private new Rigidbody2D rigidbody;
    private Vector2 velocity;

    private void Start()
    {
        jumpCooldown = Random.Range(2f, 6f);
        mario = GameObject.FindGameObjectWithTag("Player");
        venoomba = GetComponent<Venoomba>();
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void Jump()
    {
        velocity.y = jumpForce;
        jumpCooldown = Random.Range(2f, 6f);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnBecameVisible()
    {
        activated = true;
        enabled = true;
        visible = true;
    }

    private void OnBecameInvisible()
    {
        if (!activated) {
            enabled = false;
        }else {
            enabled = true;
        }
        visible = false;
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void OnEnable()
    {
        if (rigidbody != null)
        {
            rigidbody.WakeUp();
        }
    }

    private void FixedUpdate()
    {
        if (activated) {

            if (direction == Vector2.left && rigidbody.BlockRaycast(Vector2.left, "Venoomba")) {
                Jump();
            }else if (direction == Vector2.right && rigidbody.BlockRaycast(Vector2.right, "Venoomba")) {
                Jump();
            }

            if (jumpCooldown > 0f && GetComponent<Venoomba>().alive && visible)
            {
                jumpCooldown -= Time.deltaTime;
            }
            if (jumpCooldown > 0.5f || jumpCooldown < 0f) {
                HorizontalMovement();
            }else {
                if (visible) {
                    GetComponent<AnimatedSprite>().enabled = false;
                    velocity.x = 0f;
                    /*if (jumpCooldown < 0.5f && jumpCooldown > 0.25f && GetComponent<Shake>().shaking == false) {
                        GetComponent<Shake>().StartShaking();
                    }*/
                }else {
                    HorizontalMovement();
                }
            }

            grounded = rigidbody.VenoombaRaycast(Vector2.down);
            if (grounded && jumpCooldown <= 0f)
            {
                Jump();
            }
            ApplyGravity();

            
            velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

            if (rigidbody.VenoombaRaycast(Vector2.down))
            {
                velocity.y = Mathf.Max(velocity.y, 0f);
            }

            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void HorizontalMovement() {
        GetComponent<AnimatedSprite>().enabled = true;
        if (visible) {
                speed = 3.5f;
            }else {
                speed = 7f;
            }
        if (mario.GetComponent<PlayerMovement>().enabled) {
            if (mario.transform.position.x < transform.position.x)
            {
                direction = Vector2.left;
            }else if (mario.transform.position.x > transform.position.x)
            {
                direction = Vector2.right;
            }else
            {
                direction = Vector2.zero;
            }
        }

        bool orgn = true;
        if (velocity.x > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            orgn = false;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = Vector3.zero;
            orgn = true;
        }else {
            GetComponent<AnimatedSprite>().enabled = false;
            if (orgn) {
                transform.eulerAngles = Vector3.zero;
            }else {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }

        velocity.x = direction.x * speed;
    }
}
