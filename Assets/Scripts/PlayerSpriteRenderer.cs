using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    private PlayerMovement movement;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public Sprite squat;
    public AnimatedSprite run;
    private Player player;

    public FlagPole flag;
    public PoleAnimation poleAnim;
    public bool endCutscene;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (flag != null) {
            if (flag.onPole || flag.toCastle)
            {
                endCutscene = true;
            }else
            {
                endCutscene = false;
            }
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate()
    { 
        if (!endCutscene)
        {
            run.enabled = movement.running;

            if (movement.squatting && !movement.jumping && squat != null)
            {
                spriteRenderer.sprite = squat;
                player.Squat();
            }
            else if (movement.squatting && movement.jumping && squat != null)
            {
                spriteRenderer.sprite = squat;
                player.Squat();
            }
            else if (!movement.squatting && movement.jumping)
            {
                spriteRenderer.sprite = jump;
                if (player.big || player.fire)
                {
                    player.Stand();
                }
            }
            else if (movement.sliding)
            {
                spriteRenderer.sprite = slide;
                if (player.big || player.fire)
                {
                    player.Stand();
                }
            }
            else if (!movement.running)
            {
                spriteRenderer.sprite = idle;
                if (player.big || player.fire)
                {
                    player.Stand();
                }
            }
        }else
        {
            if (flag.onPole == true && flag.toCastle == false)
            {
                poleAnim.enabled = true;
                run.enabled = false;
            }else if (flag.onPole == false && flag.toCastle == true)
            {
                poleAnim.enabled = false;
                run.enabled = true;
                if (player.small) {
                    poleAnim.enabled = false;
                    run.enabled = true;
                }
            }else
            {
                Debug.Log("Smth went wrong!");
            }
        }
    }
}
