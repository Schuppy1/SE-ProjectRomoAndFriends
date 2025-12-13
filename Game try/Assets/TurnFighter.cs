using UnityEngine;

public class TurnFighter : MonoBehaviour
{
    [Header("Turn")]
    public bool isMyTurn;

    [Header("Movement")]
    public float moveDistance = 2f;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Rigidbody2D rb;
    public Animator anim;

    public int facingDirection = 1;
    private float targetX;
    private float horizontal;
    private bool isDashing;

    [Header("Combat")]
    public GameObject meeleeTrig;
    public FireballSumonnerVoice fireball;
    private float punchDuration = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetX = rb.position.x;

        fireball.player = this;
    }

    public void SetTurn(bool active)
    {
        isMyTurn = active;

        if (!isMyTurn)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("horizontal", 0);
        }
    }

    private void FixedUpdate()
    {
        if (!isMyTurn || isDashing) return;

        float newX = Mathf.MoveTowards(rb.position.x, targetX, moveSpeed * Time.fixedDeltaTime);
        rb.position = new Vector2(newX, rb.position.y);
        anim.SetFloat("horizontal", horizontal);
    }

    // ===== ACTIONS (CALLED BY VOICE) =====

    public void MoveLeft()
    {
        if (!isMyTurn) return;
        targetX -= moveDistance;
        horizontal = -1;
        facingDirection = 0;
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void MoveRight()
    {
        if (!isMyTurn) return;
        targetX += moveDistance;
        horizontal = 1;
        facingDirection = 1;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Jump()
    {
        if (!isMyTurn) return;
        anim.SetTrigger("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        if (!isMyTurn) return;
        isDashing = true;
        anim.SetTrigger("Dash");

        float dir = facingDirection == 1 ? 1 : -1;
        rb.position += Vector2.right * dir * 3f;
        targetX = rb.position.x;

        Invoke(nameof(EndDash), 0.1f);
    }

    void EndDash() => isDashing = false;

    public void Punch()
    {
        if (!isMyTurn) return;
        anim.SetTrigger("Punch");
        meeleeTrig.SetActive(true);
        Invoke(nameof(DisablePunch), punchDuration);
    }

    void DisablePunch() => meeleeTrig.SetActive(false);

    public void Fireball()
    {
        if (!isMyTurn) return;
        anim.SetTrigger("FireBall");
        fireball.SummonFireball();
    }
}
