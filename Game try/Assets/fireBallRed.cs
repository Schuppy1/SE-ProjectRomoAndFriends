using UnityEngine;

public class FireBallRed : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    public Animator anim;

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;      // assign explosion prefab
    public float explosionDuration = 1f;    // public: destroy explosion after X seconds

    private bool isDestroyed = false;

    private void Start()
    {
        FlipSprite();
        Destroy(gameObject, 5f); // auto destroy fireball after 5s
    }

    void FlipSprite()
    {
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if (!isDestroyed)
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall") || collision.collider.CompareTag("FireBall"))
        {
            // Damage player only
            if (collision.collider.CompareTag("Player"))
            {
                HPMechanics hp = collision.collider.GetComponent<HPMechanics>();

                if (hp != null)
                {
                    hp.takeDamangeRanged(transform.position); // pass fireball position for knockback
                }
            }

            Explode();
        }
    }

    private void Explode()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        // Spawn explosion prefab
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionDuration); // destroy after public delay
        }

        // Optional: play animator explosion
        if (anim != null)
        {
            anim.SetTrigger("Explode");
        }

        // Destroy fireball immediately (or short delay to play anim)
        Destroy(gameObject, 0.05f);
    }
}
