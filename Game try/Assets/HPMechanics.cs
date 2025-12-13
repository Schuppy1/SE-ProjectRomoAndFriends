using System.Collections;
using UnityEngine;

public class HPMechanics : MonoBehaviour
{
    public int HP = 100;
    public SpriteRenderer spriteRenderer;      // assign player's sprite
    public float hitFlashDuration = 0.2f;      // red flash duration
    public float knockbackForce = 5f;          // public knockback strength
    public float knockbackDuration = 0.1f;     // how long the knockback lasts

    private Rigidbody2D rb;
    public static bool canMove = true; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (HP == 0)
        {
            spriteRenderer.color = Color.black;
            canMove = false;
            Time.timeScale = 0f;
            //show win UI
            //show restart and quit button
        }
        else
            canMove = true;
    }


    public void takeDamangeRanged(Vector2 attackSourcePosition)
    {
        HP -= 10;

        // Flash red
        StartCoroutine(HitFlash());

        // Apply knockback and disable movement
        StartCoroutine(Knockback(attackSourcePosition));
    }

    public void takeDamageMeelee(Vector2 attackSourcePosition)
    {
        HP -= 20;

        // Flash red
        StartCoroutine(HitFlash());

        // Apply knockback and disable movement
        StartCoroutine(Knockback(attackSourcePosition));
    }

    private IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(hitFlashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator Knockback(Vector2 sourcePosition)
    {
        if (rb != null)
        {
            canMove = false; 

            Vector2 knockDir = (Vector2)(transform.position) - sourcePosition;
            knockDir.Normalize();

            float timer = 0f;
            while (timer < knockbackDuration)
            {
                rb.linearVelocity = knockDir * knockbackForce;
                timer += Time.deltaTime;
                yield return null;
            }

            // Stop knockback
            rb.linearVelocity = Vector2.zero;
            canMove = true; // enable movement again
        }
    }
}
