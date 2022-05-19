using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    public void PlayAgain () {
        Application.LoadLevel("Menu");
    }

    public void Play () {
        Application.LoadLevel("Level");
    }

    public void Exit () {
        Application.Quit ();
    }
}
