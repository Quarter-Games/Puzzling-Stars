using UnityEngine;

public class DrawingLine : MonoBehaviour
{
    [SerializeField] LineRenderer LineRenderer;
    [SerializeField] Camera MainCamera;
    private void Awake()
    {
        MainCamera = Camera.main;
    }
    void Update()
    {
        if (Star.selectedStar == null)
        {
            LineRenderer.positionCount = 0;
            return;
        }
        LineRenderer.positionCount = 2;
        Vector3 pos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        LineRenderer.SetPosition(0, Star.selectedStar.transform.position);
        LineRenderer.SetPosition(1, pos);

    }
}
