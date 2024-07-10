using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private LevelSettings lastLoadedLevel;
    private void OnEnable()
    {
        LevelManager.OnLevelComplete += OnLevelComplete;
        CameraControll.OnLevelLoad += OnLevelLoad;
    }

    private void OnLevelLoad(LevelSettings settings)
    {
        lastLoadedLevel = settings;
    }

    private void OnLevelComplete()
    {
        var comp = PlayerPrefs.GetInt(lastLoadedLevel.SceneName, 0);
        PlayerPrefs.SetInt(lastLoadedLevel.SceneName, comp == 2 ? 2 : 1);
    }

    private void OnDisable()
    {
        LevelManager.OnLevelComplete -= OnLevelComplete;
        CameraControll.OnLevelLoad -= OnLevelLoad;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main Menu");
    }
}
