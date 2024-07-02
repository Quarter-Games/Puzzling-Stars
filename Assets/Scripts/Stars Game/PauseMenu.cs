using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject GameEndWindow;

    private void OnEnable()
    {
        LevelManager.OnLevelComplete += OnLevelComplete;
    }

    private void OnLevelComplete()
    {
        GameEndWindow.SetActive(true);
    }

    public void OnMainMenuButtonClicked()
    {
        GameEndWindow.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
    private void OnDisable()
    {
        LevelManager.OnLevelComplete -= OnLevelComplete;
    }
}
