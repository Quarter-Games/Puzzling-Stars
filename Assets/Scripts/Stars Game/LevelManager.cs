using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelComplete;
    [SerializeField] List<Constellation> ConstellationList;

    private void Awake()
    {
        ConstellationList = FindObjectsByType<Constellation>(FindObjectsSortMode.None).ToList();
    }
    private void OnEnable()
    {
        StarConnection.OnConnectionCreated += OnConnectionChanged;
        StarConnection.OnConnectionDestroyed += OnConnectionChanged;
    }

    private void OnConnectionChanged(StarConnection connection)
    {
        Debug.Log("Connection created");
        foreach (var constellation in ConstellationList)
        {
            if (constellation.isFinished) continue;
            if (constellation.CheckConstellation())
            {
                Debug.Log("Constellation is complete");
                constellation.DisableConstellation();
            }
        }
        if (ConstellationList.All(x => x.isFinished))
        {
            Debug.Log("All constellations are complete");
            OnLevelComplete?.Invoke();
        }
    }


    private void OnDisable()
    {
        StarConnection.OnConnectionCreated -= OnConnectionChanged;
        StarConnection.OnConnectionDestroyed -= OnConnectionChanged;
    }
}
