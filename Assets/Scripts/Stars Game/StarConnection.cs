using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarConnection : MonoBehaviour
{
    public static List<StarConnection> connections = new List<StarConnection>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] PolygonCollider2D polygonCollider;
    private List<Star> ConnectedStars;
    private void Awake()
    {
        connections.Add(this);
    }
    public static StarConnection CreateConnection(Star[] Stars)
    {
        if (Stars.Length != 2)
        {
            Debug.LogError("StarConnection must have 2 stars");
            return null;
        }
        var ExistingConnection = connections.Find(c => c.ConnectedStars.Contains(Stars[0]) && c.ConnectedStars.Contains(Stars[1]));
        if (ExistingConnection != null)
        {
            Debug.LogWarning("Connection already exists");
            return null;
        }
        var connection = Instantiate(Resources.Load<StarConnection>("StarConnection"));
        connection.SetStars(Stars);
        return connection;
    }
    public void SetStars(Star[] Stars)
    {
        if (Stars.Length != 2)
        {
            Debug.LogError("StarConnection must have 2 stars");
            Destroy(gameObject);
            return;
        }
        Vector3 vector3 = Quaternion.Euler(0, 0, 90) * (Stars[1].transform.position - Stars[0].transform.position).normalized;
        vector3 *= (lineRenderer.startWidth/2);
        ConnectedStars = new List<Star>(Stars);
        lineRenderer.SetPosition(0, Stars[0].transform.position);
        lineRenderer.SetPosition(1, Stars[1].transform.position);
        polygonCollider.points = new Vector2[] {
            Stars[0].transform.position - vector3 - transform.position, Stars[1].transform.position - vector3 - transform.position,
            Stars[1].transform.position + vector3 - transform.position, Stars[0].transform.position + vector3 - transform.position };
    }
    public static void ClearConnection(Star[] Stars)
    {
        if (Stars.Length != 2)
        {
            Debug.LogError("StarConnection must have 2 stars");
            return;
        }
        var connection = connections.Find(c => c.ConnectedStars.Contains(Stars[0]) && c.ConnectedStars.Contains(Stars[1]));
        if (connection != null)
        {
            Destroy(connection.gameObject);
        }
    }
    public static void ClearConnectionsFromStar(Star star)
    {
        var connectionsToRemove = connections.FindAll(c => c.ConnectedStars.Contains(star));
        foreach (var connection in connectionsToRemove)
        {
            Destroy(connection.gameObject);
        }
    }
    private void OnDestroy()
    {
        connections.Remove(this);
    }
    private void OnMouseUpAsButton()
    {
        ClearConnection(ConnectedStars.ToArray());
    }
}
