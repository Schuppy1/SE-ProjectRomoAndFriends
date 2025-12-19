using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
public class sceneLoader : MonoBehaviour
{

    public int num;
    KeywordRecognizer recognizer;

    void Start()
    {
        recognizer = new KeywordRecognizer(new string[] { "start","button", "voice" });
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text.ToLower() == "start" && num == 1)
        {
            SceneManager.LoadScene("Menu - ChooseControlMethod");
        }
        string word = args.text.ToLower();

        if (word == "button" )
        {
            SceneManager.LoadScene("Menu - HeropickButton");
        }

        if (word == "voice" )
        {
            SceneManager.LoadScene("Menu - HeropickVoice");
        }
    }

    void OnDestroy()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.Stop();
            recognizer.Dispose();
        }
    }

    public void LoadScene()
    {
        if(num == 1)
        {
            SceneManager.LoadScene("Menu - ChooseControlMethod");
        }

        if (num == 2)
        {
            SceneManager.LoadScene("Menu - HeropickButton");
        }

        if (num == 3)
        {
            SceneManager.LoadScene("Menu - HeropickVoice");
        }


        //e change ni into what type of character, each player wants
        //for now, loadScene lang sa, 
        //the next kay another script nani
        if (num == 4 || num == 5)
        {
            SceneManager.LoadScene("ModernControls");
        }

        if (num == 6 || num == 7)
        {
            SceneManager.LoadScene("VoiceInput");
        }
    }



}