using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public ParticleSystem extraLifeParticle;
    public ParticleSystem mushroomParticle;
    public ParticleSystem fireParticle;
    public ParticleSystem starParticle;
    public ParticleSystem coinParticle;
    private bool once = true;

    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        FireFlower,
        StarPower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                if (coinParticle != null && once)
                {
                    var em = coinParticle.emission;
                    em.enabled = true;
                    coinParticle.Play();

                    once = false;
                }
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                if (extraLifeParticle != null && once)
                {
                    var em = extraLifeParticle.emission;
                    em.enabled = true;
                    extraLifeParticle.Play();

                    once = false;
                }
                break;

            case Type.MagicMushroom:
                if (player.GetComponent<Player>().small)
                {
                    player.GetComponent<Player>().Grow();
                }
                if (mushroomParticle != null && once)
                {
                    var em = mushroomParticle.emission;
                    em.enabled = true;
                    mushroomParticle.Play();

                    once = false;
                }
                break;

            case Type.FireFlower:
                if (player.GetComponent<Player>().small) {
                    player.GetComponent<Player>().wasSmall = true;
                    player.GetComponent<Player>().FirePower();
                }else if (player.GetComponent<Player>().big) {
                    player.GetComponent<Player>().wasSmall = false;
                    player.GetComponent<Player>().FirePower();
                }

                if (fireParticle != null && once)
                {
                    var em = fireParticle.emission;
                    em.enabled = true;
                    fireParticle.Play();

                    once = false;
                }
                break;

            case Type.StarPower:
                player.GetComponent<Player>().StarPower();
                if (starParticle != null && once)
                {
                    var em = starParticle.emission;
                    em.enabled = true;
                    starParticle.Play();

                    once = false;
                }
                break;
        }
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        if (GetComponent<BoxCollider2D>() != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if (GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 3f);
    }
}
