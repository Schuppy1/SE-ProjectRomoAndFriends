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
    private Vector2 targetPosition;

    [Header("Movement Settings")]
    public float moveDistance = 2f; // distance to move left/right
    public float moveSpeed = 5f;    // speed of horizontal movement
    public float jumpForce = 5f;    // force to jump

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;

        // Add voice commands
        actions.Add("right", MoveRight);
        actions.Add("left", MoveLeft);
        actions.Add("jump", Jump);
      
        // Start keyword recognizer
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("Recognized: " + speech.text);
        string command = speech.text.ToLower();
        if (actions.ContainsKey(command))
        {
            actions[command].Invoke();
        }
    }

    private void FixedUpdate()
    {
        rb.position = Vector2.Lerp(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }

    private void MoveRight()
    {
        targetPosition += Vector2.right * moveDistance;
    }

    private void MoveLeft()
    {
        targetPosition += Vector2.left * moveDistance;
    }

    private void Jump()
    {
       
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }
    
}
