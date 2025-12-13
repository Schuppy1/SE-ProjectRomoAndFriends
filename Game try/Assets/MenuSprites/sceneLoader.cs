using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneLoader : MonoBehaviour
{

    public int num;
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