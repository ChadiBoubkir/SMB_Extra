using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;

    public Sprite emptyBlock;
    private GameObject mario;
    public ParticleSystem particles;

    public int maxHits = -1;
    private bool animating;
    public bool once = true;

    private void Start()
    {
        mario = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }

    private void Update()
    {
        if (GetComponent<SpriteRenderer>().sprite == null)
        {
            maxHits = 1;
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.usedByEffector = true;
            box.offset = new Vector2(0f, -0.25f);
            box.size = new Vector2(1f, 0.5f);

            if (GetComponent<PlatformEffector2D>() != null)
            {
                PlatformEffector2D platformEffector = GetComponent<PlatformEffector2D>();
                platformEffector.enabled = true;
            }
        }
        else
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            box.usedByEffector = false;
            box.offset = new Vector2(0f, -0f);
            box.size = new Vector2(1f, 1f);

            if (GetComponent<PlatformEffector2D>() != null)
            {
                PlatformEffector2D platformEffector = GetComponent<PlatformEffector2D>();
                platformEffector.enabled = false;
            }
        }
    }

    private void Hit()
    {
        if ((mario.GetComponent<Player>().small) || (mario.GetComponent<Player>().big && maxHits > 0) || (mario.GetComponent<Player>().fire && maxHits > 0))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;

            maxHits--;

            if (maxHits == 0)
            {
                spriteRenderer.sprite = emptyBlock;
            }

            if (item != null)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }

            StartCoroutine(Animate());
        }
        else if ((mario.GetComponent<Player>().big && maxHits < 0) || (mario.GetComponent<Player>().fire && maxHits < 0))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            if (particles != null && once)
            {
                var em = particles.emission;

                em.enabled = true;
                particles.Play();

                once = false;
            }
            Destroy(gameObject, 3f);
        }
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
}
