using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _creditsMenuPost;


    public void OpenCredits()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
        _creditsMenuPost.SetActive(true);
    }


    public void CloseCredits()
    {
        _mainMenu.SetActive(true);
        _creditsMenu.SetActive(false);
        _creditsMenuPost.SetActive(false);
    }

    public void StartGame(int difficultyType)
    {
        PlayerPrefs.SetInt("DifficultyType", difficultyType);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
