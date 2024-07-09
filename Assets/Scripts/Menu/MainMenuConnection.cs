using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuConnection : MonoBehaviour
{
    [SerializeField] List<GameObject> Stars;
    [SerializeField] LineRenderer lineRenderer;
    private void OnValidate()
    {
        lineRenderer.positionCount = Stars.Count;
        lineRenderer.SetPositions(Stars.Select(x => x.transform.position).ToArray());
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
