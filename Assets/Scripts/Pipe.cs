using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;

    public GameObject top;
    public GameObject bottom;
    public bool destroy = false;
    private bool finished = false;
    private bool disappeared = false;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))
        {
            if (Input.GetKey(enterKeyCode))
            {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enterPosition = transform.position + enterDirection;
        Vector3 enterScale = Vector3.one * 0.5f;

        yield return Move(player, enterPosition, enterScale);
        yield return new WaitForSeconds(1f);

        bool underground = connection.position.y < 0f;
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
        finished = true;
        Disappear();

        
    }

    private void Disappear() {
        if (destroy && finished) {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade() {

        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                if (top.GetComponent<SpriteRenderer>().color.a > 0f) {
                    top.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.1f);
                }else {
                    disappeared = true;
                }
            }

            yield return null;
        }
        
    }

    void Update() {
        bottom.GetComponent<SpriteRenderer>().color = top.GetComponent<SpriteRenderer>().color;
        if(top != null && bottom != null && top.GetComponent<SpriteRenderer>().color.a == 0f) {
            DestroyImmediate(gameObject);
        }
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
