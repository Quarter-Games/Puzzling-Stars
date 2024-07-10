using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarConnection : MonoBehaviour
{
    public static event System.Action<StarConnection> OnConnectionCreated;
    public static event System.Action<StarConnection> OnConnectionDestroyed;
    public static List<StarConnection> connections = new List<StarConnection>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] PolygonCollider2D polygonCollider;
    public ConnectionDef ConnectedStars { get; private set; }
    private Star star0;
    private Star star1;
    private void OnEnable()
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
        ConnectedStars = new(Stars[0], Stars[1]);
        star0 = ConnectedStars.FirstStar;
        star1 = ConnectedStars.SecondStar;
        UpdateVisual();
        OnConnectionCreated?.Invoke(this);
    }
    [ContextMenu("Update Visual")]
    public void UpdateVisual()
    {

        Vector3 vector3 = Quaternion.Euler(0, 0, 90) * (star1.transform.position - star0.transform.position).normalized;
        vector3 *= (lineRenderer.startWidth / 2);
        lineRenderer.SetPosition(0, star0.transform.position);
        lineRenderer.SetPosition(1, star1.transform.position);
        polygonCollider.points = new Vector2[] {
            star0.transform.position - vector3 - transform.position, star1.transform.position - vector3 - transform.position,
            star1.transform.position + vector3 - transform.position, star0.transform.position + vector3 - transform.position };
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
    private void OnDisable()
    {
        connections.Remove(this);
        OnConnectionDestroyed?.Invoke(this);
    }
    private void OnMouseUpAsButton()
    {
        ClearConnection(new Star[] { ConnectedStars.FirstStar, ConnectedStars.SecondStar });
    }
    public void Disable()
    {
        polygonCollider.enabled = false;
        enabled = false;
    }
}
