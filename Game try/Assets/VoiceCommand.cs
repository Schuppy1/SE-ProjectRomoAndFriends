using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq; 

public class VoiceCommand : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        // Define cue words and what they do
        keywords.Add("punch", () => Debug.Log("Punch detected!"));
        keywords.Add("kick", () => Debug.Log("Kick detected!"));
        keywords.Add("jump", () => Debug.Log("Jump detected!"));

        // Convert dictionary keys to a string array
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();

        Debug.Log("Voice recognizer started. Say: 'punch', 'kick', or 'jump'.");
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log($"You said: {args.text}");
        keywords[args.text].Invoke();
    }

    void OnApplicationQuit()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}
