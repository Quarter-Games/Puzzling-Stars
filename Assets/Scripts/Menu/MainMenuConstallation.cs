using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuConstallation : MonoBehaviour
{
    public static event Action<MainMenuConstallation> OnConstallationClick;
    [SerializeField] List<MainMenuConnection> connections;
    public Transform ZoomInPoint;
    public LevelSettings settings;
    private void OnEnable()
    {
        OnMouseExit();
    }
    private void OnMouseEnter()
    {
        foreach (var item in connections)
        {
            item.Activate();
        }
    }
    private void OnMouseExit()
    {
        foreach (var item in connections)
        {
            item.Deactivate();
        }
    }
    private void OnMouseUpAsButton()
    {
        OnConstallationClick?.Invoke(this);
    }
}
