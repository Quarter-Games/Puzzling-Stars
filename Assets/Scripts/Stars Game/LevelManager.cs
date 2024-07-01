using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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
    }


    private void OnDisable()
    {
        StarConnection.OnConnectionCreated -= OnConnectionChanged;
        StarConnection.OnConnectionDestroyed -= OnConnectionChanged;
    }
}
