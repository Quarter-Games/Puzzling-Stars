using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuRotationButton : MonoBehaviour, IPointerEnterHandler
{
    public Vector3 Axis;
    public float Angle;
    public CameraControll _camera;
    public float Duration;
    public float timeTick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        _camera.Rotate(this);
    }
    
}
