using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer fireRenderer;
    private PlayerSpriteRenderer activeRenderer;
    private DeathAnimation deathAnimation;

    public CapsuleCollider2D capsuleCollider;

    private float shootCooldown = 1.5f;
    private float currentShootCooldown = 1.5f;

    public bool wasSmall = true;
    public bool fire => fireRenderer.enabled;
    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    private float stateDelay = 0.25f;
    public bool starpower { get; private set; }

    public bool invincible;

    private void Awake()
    {
        activeRenderer = smallRenderer;
        deathAnimation = GetComponent<DeathAnimation>();

        if (GameManager.Instance != null) {
            if (GameManager.Instance.small) {
                smallRenderer.enabled = true;
                bigRenderer.enabled = false;
                fireRenderer.enabled = false;
                activeRenderer = smallRenderer;

                capsuleCollider.size = new Vector2(1f, 1f);
                capsuleCollider.offset = new Vector2(0f, 0f);
            }else if (GameManager.Instance.big) {
                smallRenderer.enabled = false;
                bigRenderer.enabled = true;
                fireRenderer.enabled = false;
                activeRenderer = bigRenderer;

                capsuleCollider.size = new Vector2(1f, 2f);
                capsuleCollider.offset = new Vector2(0f, 0.5f);
            }else if (GameManager.Instance.fire) {
                smallRenderer.enabled = false;
                bigRenderer.enabled = false;
                fireRenderer.enabled = true;
                activeRenderer = fireRenderer;

                capsuleCollider.size = new Vector2(1f, 2f);
                capsuleCollider.offset = new Vector2(0f, 0.5f);
            }
        }
    }

    private void Update()
    {
        if (stateDelay > 0f) {
            stateDelay -= Time.deltaTime;
        }
        if (stateDelay <= 0f) {
            if (GameManager.Instance != null) {
                if (small) {
                    GameManager.Instance.small = true;
                    GameManager.Instance.big = false;
                    GameManager.Instance.fire = false;
                }else if (big) {
                    GameManager.Instance.small = false;
                    GameManager.Instance.big = true;
                    GameManager.Instance.fire = false;
                }else if (fire) {
                    GameManager.Instance.small = false;
                    GameManager.Instance.big = false;
                    GameManager.Instance.fire = true;
                }
            }
        }

        if (small)
        {
            capsuleCollider.size = new Vector2(1f, 1f);
            capsuleCollider.offset = new Vector2(0f, 0f);
        }
    }

    public void Hit()
    {
        if (!invincible && !starpower && !dead)
        {
            if (fire) {
                DeFire();
            }else if (big)
            {
                Shrink();
            }
            else if (small)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;
        deathAnimation.enabled = true;

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (GameManager.Instance != null) {
                GameManager.Instance.ResetLevel(3f);
            }
        }
    }

    public void FirePower() {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = true;
        activeRenderer = fireRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(FireAnimation());
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        fireRenderer.enabled = false;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void DeFire()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        fireRenderer.enabled = false;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);
        StartCoroutine(DeFireAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);
        StartCoroutine(ScaleAnimation());
    }

    public void Squat()
    {
        capsuleCollider.size = new Vector2(1f, 1.38f);
        capsuleCollider.offset = new Vector2(0f, 0.44f);
    }

    public void Stand()
    {
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            invincible = true;

            if (Time.frameCount % 3 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        invincible = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;

        activeRenderer.enabled = true;
    }

    private IEnumerator DeFireAnimation()
    {
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            invincible = true;

            if (Time.frameCount % 3 == 0)
            {
                fireRenderer.enabled = !fireRenderer.enabled;
                bigRenderer.enabled = !fireRenderer.enabled;
            }

            yield return null;
        }

        invincible = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;

        activeRenderer.enabled = true;
    }

    private IEnumerator FireAnimation()
    {
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            invincible = true;

            if (Time.frameCount % 3 == 0)
            {
                if (wasSmall) {
                    smallRenderer.enabled = !smallRenderer.enabled;
                    fireRenderer.enabled = !smallRenderer.enabled;
                }else if (!wasSmall) {
                    bigRenderer.enabled = !bigRenderer.enabled;
                    fireRenderer.enabled = !bigRenderer.enabled;
                }
            }

            yield return null;
        }

        invincible = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        fireRenderer.enabled = false;

        activeRenderer.enabled = true;
    }

    public void StarPower()
    {
        StartCoroutine(StarPowerAnimation());
    }

    private IEnumerator StarPowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 2f, 4f, 2f, 4f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

    void FixedUpdate() {
        currentShootCooldown -= Time.deltaTime;

        if (fire && !GetComponent<PlayerMovement>().squatting && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))) {
            if (currentShootCooldown <= 0f) {
                GetComponent<ThrowFire>().Throw();
            }else {
                return;
            }
            currentShootCooldown = shootCooldown;
        }
    }
}