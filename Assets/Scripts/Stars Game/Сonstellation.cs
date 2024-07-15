using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Constellation : MonoBehaviour
{
    public event Action<Constellation> OnConstellationComplete;
    public bool isFinished;
    public List<ConnectionDef> Connections;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Image imageInsteadSprite;
    public Constellation CopiedFrom;
    [SerializeField] Material StartResolveMat;
    [SerializeField] Material EndResolveMat;
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
        OnConstellationComplete?.Invoke(this);
    }
    public void LeaveOnlyImage(Constellation constellation)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (child.gameObject != imageInsteadSprite.gameObject)
            {
                StartCoroutine(TurnOffStarsOrLines(child.gameObject));
            }
            else
            {
                StartCoroutine(TurnOnImage(child.gameObject));

            }
        }
        constellation.OnConstellationComplete -= LeaveOnlyImage;
    }
    private IEnumerator TurnOffStarsOrLines(GameObject gameObject)
    {
        if (gameObject.transform.GetChild(0).gameObject.TryGetComponent<Image>(out var image))
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.fixedDeltaTime;
                image.color = Color.Lerp(Color.white, Color.clear, t);
                yield return new WaitForFixedUpdate();
            }

        }
        else if (gameObject.transform.GetChild(0).gameObject.TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.fixedDeltaTime;
                lineRenderer.startColor = Color.Lerp(Color.white, Color.clear, t);
                lineRenderer.endColor = Color.Lerp(Color.white, Color.clear, t);
                yield return new WaitForFixedUpdate();
            }
        }
        DestroyImmediate(gameObject);
    }
    private IEnumerator TurnOnImage(GameObject gameObject)
    {
        gameObject.SetActive(true);
        if (gameObject.TryGetComponent<Image>(out var image))
        {
            Debug.Log("lol");
            float t = 0;
            while (t < 1f)
            {
                t += Time.fixedDeltaTime;
                
                image.material.Lerp(StartResolveMat, EndResolveMat, t);
                yield return new WaitForFixedUpdate();
            }

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
            if (obj.transform.childCount == 0 && obj.transform.parent.childCount >= 2) temp.imageInsteadSprite = imagerenderer;
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
        temp.CopiedFrom = this;
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
