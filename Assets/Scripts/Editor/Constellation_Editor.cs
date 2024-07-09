using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Constellation))]
public class Constellation_Editor : Editor
{
    List<Star> Stars = new List<Star>();
    Star selectedStar;
    private void Awake()
    {
        Stars = FindObjectsByType<Star>(FindObjectsSortMode.None).ToList();
    }
    private void OnSceneGUI()
    {
        Constellation constellation = target as Constellation;
        foreach (var star in Stars)
        {
            if (selectedStar == star)
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(star.transform.position, Vector3.forward, 0.5f);
            }
            else
            {
                Handles.color = Color.green;
                var wasSelected = selectedStar;
                var isSelected = Handles.Button(star.transform.position, Quaternion.identity, 0.5f, 0.5f, Handles.CircleHandleCap);
                if (isSelected)
                {
                    selectedStar = star;
                }
                if (wasSelected != selectedStar)
                {
                    if (selectedStar == null || wasSelected == null) continue;
                    if (constellation.Contains(wasSelected, selectedStar))
                    {
                        constellation.Connections.RemoveAll(x => x.Contains(wasSelected) && x.Contains(selectedStar));
                    }
                    else constellation.Connections.Add(new ConnectionDef(wasSelected, selectedStar));
                }
                else if (wasSelected == selectedStar && isSelected)
                {
                    selectedStar = null;
                    
                }
            }
        }
    }
}
