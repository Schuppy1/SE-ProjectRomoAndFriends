using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommanV2 : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> actions = new();

    public TurnManager turnManager;

    private void Start()
    {
        actions.Add("left", () => Execute(p => p.MoveLeft()));
        actions.Add("right", () => Execute(p => p.MoveRight()));
        actions.Add("jump", () => Execute(p => p.Jump()));
        actions.Add("dash", () => Execute(p => p.Dash()));
        actions.Add("punch", () => Execute(p => p.Punch()));
        actions.Add("fireball", () => Execute(p => p.Fireball()));

        recognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnSpeech;
        recognizer.Start();
    }

    void Execute(Action<TurnFighter> action)
    {
        TurnFighter active = turnManager.ActivePlayer();
        action(active);
        turnManager.ExecuteAction();
    }

    private void OnSpeech(PhraseRecognizedEventArgs speech)
    {
        string cmd = speech.text.ToLower();
        if (actions.ContainsKey(cmd))
            actions[cmd].Invoke();
    }

    private void OnDestroy()
    {
        recognizer?.Stop();
        recognizer?.Dispose();
    }
}

