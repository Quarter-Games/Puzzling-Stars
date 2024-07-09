using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] float Distance;
    [SerializeField] List<MainMenuRotationButton> RotationButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    [ContextMenu("Align Menu Stars")]
    public void AlignMenuStars()
    {
        var objects = GameObject.FindGameObjectsWithTag("Menu Star");
        foreach (var obj in objects)
        {
            var pos = obj.transform.position;
            obj.transform.position = pos.normalized * Distance;

        }
    }
    private void OnEnable()
    {
        MainMenuRotationButton.VerticalRotation += ChangeRotationButtonVisibility;
    }
    private void OnDisable()
    {
        MainMenuRotationButton.VerticalRotation -= ChangeRotationButtonVisibility;
    }

    private void ChangeRotationButtonVisibility()
    {
        foreach (var button in RotationButtons)
        {
            button.gameObject.SetActive(!button.gameObject.activeSelf);
        }
    }
}
