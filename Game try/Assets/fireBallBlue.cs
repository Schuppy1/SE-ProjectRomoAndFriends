using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;

    private void Start()
    {
        FlipSprite();
        Destroy(gameObject, 5f);
    }

    void FlipSprite()
    {
        // if moving right → face right
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
