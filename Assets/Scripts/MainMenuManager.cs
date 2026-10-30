using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject TutorialPopup;
    public Image fadeImage;
    public float fadeDuration = 1f;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenTutorial()
    {
        TutorialPopup.SetActive(true);
    }

    public void CloseTutorial()
    {
        TutorialPopup.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
