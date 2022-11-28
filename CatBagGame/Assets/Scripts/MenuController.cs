using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Destination
{
    Main, Pause, Resume, Credits, Quit, Gameplay, HowToPlay
}

public class MenuController : MonoBehaviour
{
    [SerializeField] Destination des;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject miniHowToPlayMenu;

    public void LoadScreen()
    {
        switch (des)
        {
            case Destination.Main:
                SceneManager.LoadScene("TitleScreen");
                break;
            case Destination.Pause:
                pauseMenu.SetActive(true);
                break;
            case Destination.Resume:
                pauseMenu.SetActive(false);
                break;
            case Destination.Credits:
                SceneManager.LoadScene("CreditsScreen");
                break;
            case Destination.Quit:
                Application.Quit();
                break;
            case Destination.Gameplay:
                SceneManager.LoadScene("Gameplay");
                break;
            case Destination.HowToPlay:
                if (SceneManager.GetActiveScene().name == "TitleScreen")
                {
                    SceneManager.LoadScene("HowToPlayScreen");
                }
                else
                {
                    miniHowToPlayMenu.SetActive(true);
                }
                break;
            default:
                Debug.Log("not working " + des);
                break;
        }
    }
}
