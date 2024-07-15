using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Constellation : MonoBehaviour
{
    public bool isFinished;
    public List<ConnectionDef> Connections;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void OnDrawGizmos()
    {
        if (Connections == null) return;
        foreach (var connection in Connections)
        {
            if (connection.FirstStar == null || connection.SecondStar == null) continue;
            Gizmos.DrawLine(connection.FirstStar.transform.position, connection.SecondStar.transform.position);
        }
    }
    public bool Contains(Star first, Star second)
    {
        if (Connections == null) return false;
        foreach (var connection in Connections)
        {
            if (connection.Contains(first) && connection.Contains(second))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckConstellation()
    {
        foreach (var connection in Connections)
        {
            if (StarConnection.connections.Find(c => c.ConnectedStars == connection) == null)
            {
                return false;
            }
            var first = connection.FirstStar.ContainedIn();
            var second = connection.SecondStar.ContainedIn();
            foreach (var starCon in first)
            {
                bool isFound = false;
                foreach (var con in Connections)
                {
                    if (con == starCon)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) return false;
            }


            foreach (var starCon in second)
            {
                bool isFound = false;
                foreach (var con in Connections)
                {
                    if (con == starCon)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) return false;
            }

        }
        return true;
    }
    public void DisableConstellation()
    {
        isFinished = true;
        foreach (var connection in Connections)
        {
            connection.FirstStar.Disable();
            connection.SecondStar.Disable();
            StarConnection.connections.Find(c => c.ConnectedStars == connection).Disable();
        }
    }
    [ContextMenu("Copy")]
    public void CreateFinishedCopy()
    {
        var temp = Instantiate(this);
        temp.gameObject.AddComponent<RectTransform>();
        foreach (var con in temp.Connections)
        {
            var connection = StarConnection.CreateConnection(new[] { con.FirstStar, con.SecondStar });
            connection.Disable();
            connection.transform.SetParent(temp.transform);
        }
        foreach (var star in temp.GetComponentsInChildren<Star>())
        {
            star.Disable();
            star.transform.localPosition = star.transform.localPosition * 100;
        }
        foreach (var sprite in temp.GetComponentsInChildren<SpriteRenderer>(true))
        {
            var obj = sprite.gameObject;
            var image = sprite.sprite;
            DestroyImmediate(sprite);
            var imagerenderer = obj.AddComponent<Image>();
            imagerenderer.sprite = image;
            imagerenderer.SetNativeSize();
        }
        foreach (var objec in temp.GetComponentsInChildren<Transform>(true))
        {
            objec.gameObject.layer = 5;
        }
        foreach (var layout in FindObjectsByType<VerticalLayoutGroup>(FindObjectsSortMode.None))
        {
            if (layout.CompareTag("EditorOnly"))
            {
                temp.transform.SetParent(layout.transform);
                temp.transform.localScale = Vector3.one * .75f;
                break;
            }
        }
        temp.Refresh();
        temp.enabled = false;
    }
    [ContextMenu("Refresh")]
    public void Refresh()
    {
        GetComponentsInChildren<StarConnection>().ToList().ForEach(x => x.UpdateVisual());
    }
}
[Serializable]
public class ConnectionDef
{
    public Star FirstStar;
    public Star SecondStar;
    public bool Contains(Star star)
    {
        return FirstStar == star || SecondStar == star;
    }
    public ConnectionDef(Star firstStar, Star secondStar)
    {
        FirstStar = firstStar;
        SecondStar = secondStar;
    }
    public static bool operator ==(ConnectionDef a, ConnectionDef b)
    {
        return (a.FirstStar == b.FirstStar && a.SecondStar == b.SecondStar) || (a.FirstStar == b.SecondStar && a.SecondStar == b.FirstStar);
    }
    public static bool operator !=(ConnectionDef a, ConnectionDef b)
    {
        return !(a == b);
    }
}
