using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ModernControls : MonoBehaviour
{

    private Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;  
    public Animator anim;

    private float horizontal = 0f;
    public int facingDirection = 1; // 1 = right, 0 = left

    //punch damage trigger
    public GameObject meeleeTrig;
    private float punchDuration = 0.1f;

    // fireball summoner
    public FireballSumonner summonFireBall;


    //jumpn'dash input
    [Header("Jump N'Dash Settings")]
    private bool canJump = true;

    private int tapCount = 0;
    public float doubleTapTime = 2f;
    public float jumpForce = 5f;
    public float dashDistance = 3f;
    public float jumpCooldown = 1.5f;
    private bool isDashing = false;
    public bool isGrounded = false;



    //for combos thingy ari
    private bool globalComboLock = false;
    [Header("Combo - 1 Settings")]
    // ===== COMBO 1 (J KEY) =====
    private bool canCombo = true;

    
    public float comboResetTime = 3f;    // time allowed to continue combo
    public float comboCooldown = 1f;     // cooldown after combos
    private int comboStep = 0;           // 0 = ready, 1 = dash, 2 = punch1, 3 = punch2
    private float comboTimer = 0f;

    public float PunchpressInterval = 0.5f;   // required delay between combo presses
    private float intervalTimer = 0f;    // tracks time after each hit
    private bool intervalReady = true;   // true = can press again

    [Header("Combo - 2 Settings")]
    // ===== COMBO 2 (K KEY) =====
    public float pressInterval2 = 0.5f;
    public float comboResetTime2 = 3f;
    public float comboCooldown2 = 2f;

    private int comboStep2 = 0;
    private float comboTimer2 = 0f;
    private bool canCombo2 = true;
    private float intervalTimer2 = 0f;
    private bool intervalReady2 = true;

    [Header("Combo - 3 Settings")]
    // ===== COMBO 2 (L KEY) =====
    public float pressInterval3 = 0.5f;
    public float comboResetTime3 = 3f;
    public float comboCooldown3 = 2f;

    private int comboStep3 = 0;
    private float comboTimer3 = 0f;
    private bool canCombo3 = true;
    private float intervalTimer3 = 0f;
    private bool intervalReady3 = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        summonFireBall = FindFirstObjectByType<FireballSumonner>();
   
    }

    private void Update()
    {
        if (HPMechanics.canMove)
        {
            HandleMovementInput();
            SpaceToDashInput();

            ShortRangeCombo();
            MediumRangeCombo();
            LongRangeCombo();
        }
        else
        {
            Debug.Log("knockedback");
        }
    }

    private void HandleMovementInput()
    {
        // Read WASD keys only
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        else if (Input.GetKey(KeyCode.A)) horizontal = -1f;

        // Apply velocity
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }

        // Set animation values
        anim.SetFloat("horizontal", horizontal);

        anim.SetBool("GLeft", horizontal < 0);
        anim.SetBool("GRight", horizontal > 0);

        // handle facing direction
        if (horizontal > 0)
        {
            facingDirection = 1;
            transform.localScale = new Vector3(-1, 1, 1);  // facing right
        }
        else if (horizontal < 0)
        {
            facingDirection = 0;
            transform.localScale = new Vector3(1, 1, 1);   // facing left
        }
    }

    //mobilty combo -----------------------------------------------------------
    void SpaceToDashInput()
    {
            if (Input.GetKeyDown(KeyCode.Space))
             {
            // FIRST PRESS → JUMP
            if (tapCount == 0)
            {
                if (!canJump) return;

                Jump();
                tapCount = 1;
            }
            // SECOND PRESS → DASH
            else if (tapCount == 1)
            {
                Dash();
                canJump = false;     // disable jump until grounded
                tapCount = 2;        // waiting for reset
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            // RESET JUMP + DASH
            tapCount = 0;
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    //---------------------------------------------------------------------------


    //---------------------------------------------------------------------------
    //combo 1
    private void ShortRangeCombo()
    {
        if (globalComboLock && comboStep == 0) return;
        if (!canCombo) return; // blocked by cooldown

        // Count interval timer if interval is active
        if (!intervalReady)
        {
            intervalTimer += Time.deltaTime;
            if (intervalTimer >= PunchpressInterval)
            {
                intervalReady = true;   // ready for next input
                intervalTimer = 0f;
            }
        }

        // Detect J press (only when interval allows)
        if (Input.GetKeyDown(KeyCode.J) && intervalReady)
        {
            //if ang combo 1 kay ga on
            if (comboStep == 0)
                globalComboLock = true;

            intervalReady = false;  // block next press temporarily
            comboStep++;
            comboTimer = 0f; // Reset combo timeout timer

            if (comboStep == 1)
            {
                Debug.Log("Combo 1: DASH");
                //display the text(Fade in n out)

                Dash();
                // Dash animation here
            }
            else if (comboStep == 2)
            {
                Debug.Log("Combo 2: PUNCH 1");
                Punch();
                // Punch 1 animation here
            }
            else if (comboStep == 3)
            {
                Debug.Log("Combo 3: PUNCH 2");
                // Punch 2 animation here
                Punch();

                StartCoroutine(StartComboCooldown());
            }
        }

        // Combo timeout logic
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= comboResetTime) // Exceeded time limit
            {
                StartCoroutine(StartComboCooldown());
            }
        }
    }

    private IEnumerator StartComboCooldown()
    {
        canCombo = false;   // block input
        comboStep = 0;      // reset combo
        comboTimer = 0f;
        intervalReady = true;
        intervalTimer = 0f;
        globalComboLock = false;

        Debug.Log("Combo 1 Cooldown started");

        yield return new WaitForSeconds(comboCooldown);

        canCombo = true;
        

        // allow combo again
        Debug.Log("Combo Ready");
    }

    //-----------------------------------------------------------------------

    //---------------------------------------------------------------------------
    //combo 2
    private void MediumRangeCombo()
    {
        if (globalComboLock && comboStep2 == 0) return; // cannot start a new combo
        if (!canCombo2) return; // cooldown

        // Interval timer
        if (!intervalReady2)
        {
            intervalTimer2 += Time.deltaTime;
            if (intervalTimer2 >= pressInterval2)
            {
                intervalReady2 = true;
                intervalTimer2 = 0f;
            }
        }

        // Input
        if (Input.GetKeyDown(KeyCode.K) && intervalReady2)
        {
            if (comboStep2 == 0)
                globalComboLock = true;  // lock when combo 2 starts

            intervalReady2 = false;
            comboStep2++;
            comboTimer2 = 0f;

            if (comboStep2 == 1)
            {
                FireBall();
            }
            else if (comboStep2 == 2)
            {

                Dash();

            }
            else if (comboStep2 == 3)
            {
                Punch();

                StartCoroutine(Combo2Cooldown());
            }
        }

        // Timeout
        if (comboStep2 > 0)
        {
            comboTimer2 += Time.deltaTime;
            if (comboTimer2 >= comboResetTime2)
            {
                StartCoroutine(Combo2Cooldown());
            }
        }
    }
    private IEnumerator Combo2Cooldown()
    {
        canCombo2 = false;

        comboStep2 = 0;
        comboTimer2 = 0f;
        intervalReady2 = true;
        intervalTimer2 = 0f;
        globalComboLock = false;

        Debug.Log("K Combo Cooldown...");

        yield return new WaitForSeconds(comboCooldown2);

        canCombo2 = true;
       

        Debug.Log("K Combo Ready!");
    }

    //--------------------------------------------------------------------



    //---------------------------------------------------------------------------
    //combo 3
    private void LongRangeCombo()
    {
        if (globalComboLock && comboStep3 == 0) return; // cannot start a new combo
        if (!canCombo3) return; // cooldown

        // Interval timer
        if (!intervalReady3)
        {
            intervalTimer3 += Time.deltaTime;
            if (intervalTimer3 >= pressInterval2)
            {
                intervalReady3 = true;
                intervalTimer3 = 0f;
            }
        }

        // Input
        if (Input.GetKeyDown(KeyCode.L) && intervalReady3)
        {
            if (comboStep3 == 0)
                globalComboLock = true;  // lock when combo 3 starts

            intervalReady3 = false;
            comboStep3++;
            comboTimer3 = 0f;

            if (comboStep3 == 1)
            {
                FireBall();
            }
            else if (comboStep3 == 2)
            {

                FireBall();

            }
            else if (comboStep3 == 3)
            {
                Dash();

                StartCoroutine(Combo3Cooldown());
            }
        }

        // Timeout
        if (comboStep3 > 0)
        {
            comboTimer3 += Time.deltaTime;
            if (comboTimer3 >= comboResetTime2)
            {
                StartCoroutine(Combo3Cooldown());
            }
        }
    }
    private IEnumerator Combo3Cooldown()
    {
        canCombo3 = false;

        comboStep3 = 0;
        comboTimer3 = 0f;
        intervalReady3 = true;
        intervalTimer3 = 0f;
        globalComboLock = false;

        Debug.Log("L Combo Cooldown...");

        yield return new WaitForSeconds(comboCooldown3);

        canCombo3 = true;

        Debug.Log("L Combo Ready!");
    }

    //--------------------------------------------------------------------


    //actions ari nga part
    private void Jump()
    {
        anim.SetTrigger("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Dash()
    {
        if (isDashing) return;

        isDashing = true;
        anim.SetTrigger("Dash");

        float dashDir = (facingDirection == 1) ? 1f : -1f;

        Vector2 newPos = new Vector2(rb.position.x + dashDir * dashDistance, rb.position.y);
        rb.position = newPos;

        Invoke(nameof(EndDash), 0.05f);
    }

    private void EndDash()
    {
        isDashing = false;
    }


    private void Punch()
    {
        anim.SetTrigger("Punch");

        meeleeTrig.SetActive(true);

        Invoke(nameof(HidePunch), punchDuration);

        //turn on punch damage trigger gameobject
    }

    private void HidePunch()
    {
        meeleeTrig.SetActive(false);
    }

    private void FireBall()
    {
        anim.SetTrigger("FireBall");
        summonFireBall.SummonFireball();
    }



}
