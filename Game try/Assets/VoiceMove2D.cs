using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class VoiceMove2D : MonoBehaviour
{
    KeywordRecognizer recognizer;
    Dictionary<string, System.Action> commands = new Dictionary<string, System.Action>();

    public float speed = 5f;
    Vector2 direction = Vector2.zero;

    void Start()
    {
        // Voice commands list
        commands.Add("up", () => direction = Vector2.up);
        commands.Add("down", () => direction = Vector2.down);
        commands.Add("left", () => direction = Vector2.left);
        commands.Add("right", () => direction = Vector2.right);
        commands.Add("stop", () => direction = Vector2.zero);

        // Setup recognizer
        recognizer = new KeywordRecognizer(commands.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnCommand;
        recognizer.Start();
    }

    void OnCommand(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Heard: " + args.text);
        commands[args.text].Invoke();
    }

    void Update()
    {
       
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

