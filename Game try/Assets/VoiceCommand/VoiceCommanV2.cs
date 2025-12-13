using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;

public class VoiceCommanV2 : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private Rigidbody2D rb;
    private float targetX;

    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Animator anim;
    private float horizontal = 0f;   // animator float (-1 left, 0 idle, 1 right)
    public int facingDirection = 0;

    [Header("Dash Settings")]
    public float dashDistance = 3f;
    public float dashCooldown = 1f;
    private bool isDashing = false;

    //punch damage trigger
    public GameObject meeleeTrig;
    private float punchDuration = 0.1f;

    //fireball summoner
    public FireballSumonnerVoice summonFireBall;

    private void Start()
    {
        summonFireBall = FindFirstObjectByType<FireballSumonnerVoice>();

        rb = GetComponent<Rigidbody2D>();
        targetX = rb.position.x;

        actions.Add("right", MoveRight);
        actions.Add("left", MoveLeft);
        actions.Add("jump", Jump);
        actions.Add("fireball", FireBall);
        actions.Add("dash", Dashh);
        actions.Add("punch", Punch);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }
   
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("Recognized: " + speech.text);

        if (actions.ContainsKey(speech.text.ToLower()))
            actions[speech.text.ToLower()].Invoke();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            float newX = Mathf.MoveTowards(rb.position.x, targetX, moveSpeed * Time.fixedDeltaTime);
            rb.position = new Vector2(newX, rb.position.y);
        }

        // Auto idle
        if (!isDashing && Mathf.Abs(rb.position.x - targetX) < 0.01f)
        {
            horizontal = 0f;
            anim.SetBool("GLeft", false);
        }

        anim.SetFloat("horizontal", horizontal);
    }


    private void MoveRight()
    {
        targetX += moveDistance;
        transform.localScale = new Vector3(-1, 1, 1);
        horizontal = 1f;   // Moving right animation
        facingDirection = 1;

    }

    private void MoveLeft()
    {
        anim.SetBool("GLeft", true);
        targetX -= moveDistance;
        transform.localScale = new Vector3(1, 1, 1);
        horizontal = -1f;  // Moving left animation
        //0 kay looking left
        facingDirection = 0;
    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Dashh()
    {

        isDashing = true; // stop movement logic temporarily

        //animation sa dash ari 
        anim.SetTrigger("Dash");

        float dashDir = (facingDirection == 1) ? 1f : -1f;

        // Teleport
        Vector2 newPosition = new Vector2(rb.position.x + dashDir * dashDistance, rb.position.y);
        rb.position = newPosition;

        // Update targetX so movement does NOT pull player back
        targetX = rb.position.x;
        anim.SetBool("GLeft", false);
        anim.SetBool("GLeft", false);

        Invoke(nameof(EndDash), 0.05f);
    }
    private void EndDash()
    {
        isDashing = false;
    }

    private void FireBall()
    {
        anim.SetTrigger("FireBall");
        summonFireBall.SummonFireball();
    }

    private void Punch()
    {
        anim.SetTrigger("Punch");

        meeleeTrig.SetActive(true);

        Invoke(nameof(HidePunch), punchDuration);

    }
    private void HidePunch()
    {
        meeleeTrig.SetActive(false);
    }


}
