using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private AnimatedSprite animSpr;
    private Player player;
    private new Collider2D collider;

    public Vector2 velocity;
    private float inputAxis;

    public float speed = 7f;

    public float maxJumpHeight = 4.35f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public float launchDistance = 4f;
    public float maxLaunchHeight = 5f;
    public float maxLaunchTime = 1f;
    public float launchForce => (2f * maxLaunchHeight) / (maxLaunchTime / 2f);
    private bool launched = false;

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool squatting { get; private set; }

    private bool notSmall = false;
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        animSpr = GetComponentInChildren<AnimatedSprite>();
        player = GetComponent<Player>();
        collider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
        squatting = false;
    }

    private void OnEnable()
    {
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
        if (collider != null)
        {
            collider.enabled = true;
        }
        velocity = Vector2.zero;
        jumping = false;
        squatting = false;
    }

    private void FixedUpdate()
    {
        if (player.small) {
            GameObject smol = GameObject.FindGameObjectWithTag("Small");
            animSpr = smol.GetComponent<AnimatedSprite>();
        }else if (player.big) {
            GameObject grand = GameObject.FindGameObjectWithTag("Big");
            animSpr = grand.GetComponent<AnimatedSprite>();
        }

        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 tempPos = position;
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        if (position != tempPos)
        {
            velocity.x = 0f;
        }
        
        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }

    private void Update()
    {   
        notSmall = player.big || player.fire;
        if (!squatting)
        {
            HorizontalMovement();
        }

        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded)
        {
            launched = false;
            GroundedMovement();
        }
        ApplyGravity();
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || player.starpower)
        {
            speed = 11f;
            animSpr.frameRate = 1f / 6f;
        }else {
            speed = 7f;
            animSpr.frameRate = 1f / 6f;
        }
        if (!launched) {
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * speed, speed * Time.deltaTime + 0.25f);
        }

        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if (!sliding)
        {
            if (velocity.x > 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if (velocity.x < 0f)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
        if (sliding)
        {
            if (inputAxis == -1)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            if (inputAxis == 1)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private void GroundedMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.S) && notSmall)
        {
            transform.position -= new Vector3(0f, 0.245f, 0f);
            squatting = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && notSmall)
        {
            transform.position += new Vector3(0f, 0.245f, 0f);
            velocity.y = 0f;
            squatting = false;
        }

        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }

        if (Input.GetKey(KeyCode.S) && notSmall)
        {
            squatting = true;
            if (velocity.x > 0)
            {
                velocity.x -= Time.deltaTime * 13;
                if (velocity.x < 0.0005f)
                {
                    velocity.x = 0;
                }
            }
            if (velocity.x < 0)
            {
                velocity.x += Time.deltaTime * 13;
                if (velocity.x > -0.0005f)
                {
                    velocity.x = 0;
                }
            }
        }else
        {
            squatting = false;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    public void Launch(Transform other) {
        launched = true;
        velocity.y = launchForce;
        if (!grounded) {
            jumping = true;
        }
        Vector2 direction = Vector2.right;
        if (other.position.x < transform.position.x) {
            direction = Vector2.right;
        }if (other.position.x > transform.position.x) {
            direction = Vector2.left;
        }else {
            if (camera.transform.position.x < transform.position.x) {
                direction = Vector2.right;
            }else if (camera.transform.position.x > transform.position.x) {
                direction = Vector2.left;
            }else {
                direction = Vector2.right;
            }
        }
        if (direction == Vector2.right) {
            velocity.x = Mathf.MoveTowards(velocity.x, direction.x * launchDistance, launchDistance);
        }
        if (direction == Vector2.left) {
            velocity.x = Mathf.MoveTowards(velocity.x, direction.x * launchDistance, -launchDistance);
        }
    }
}
