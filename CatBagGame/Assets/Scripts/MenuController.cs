using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Destination
{
    Main, Pause, Controls, Credits, Quit, Gameplay
}

public class MenuController : MonoBehaviour
{
    [SerializeField] Destination des;

    public void LoadScreen()
    {
        switch (des)
        {
            case Destination.Main:
                break;
            case Destination.Pause:
                break;
            case Destination.Controls:
                break;
            case Destination.Credits:
                break;
            case Destination.Quit:
                Application.Quit();
                break;
            case Destination.Gameplay:
                break;
            default:
                break;
        }
    }
}
