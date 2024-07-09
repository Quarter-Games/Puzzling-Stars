using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuRotationButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action VerticalRotation;
    public Vector3 Axis;
    public float Angle;
    public CameraControll _camera;
    public float Duration;
    public float timeTick;
    [SerializeField] public bool Vertical;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        if (_camera.Rotate(this) && Vertical)
        {
            VerticalRotation?.Invoke();
        }
    }

}
