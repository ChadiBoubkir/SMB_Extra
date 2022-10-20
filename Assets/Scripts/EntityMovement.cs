using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    public bool starman = false;
    public float maxBounceHeight = 6f;
    public float maxBounceTime = 4f;
    public float bounceForce => (2f * maxBounceHeight) / (maxBounceTime / 2f) + 6f;
    public float gravity => (-2f * maxBounceHeight) / Mathf.Pow((maxBounceTime / 2f), 2) - 17f;
    public bool grounded { get; private set; }

    private new Rigidbody2D rigidbody;
    private Vector2 velocity;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void Update()
    {
        if (starman)
        {
            grounded = rigidbody.Raycast(Vector2.down);
            if (grounded)
            {
                Bounce();
            }
            ApplyGravity();
        }
    }

    private void Bounce()
    {
        velocity.y = bounceForce;
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
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
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        if (gameObject.tag != "Koopa" && rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
        else if (gameObject.tag == "Koopa" && rigidbody.KoopaRaycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }

        if (gameObject.tag == "Koopa")
        {
            if (velocity.x > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if (velocity.x < 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction))
        {
            direction *= -1;
        }
    }
}
