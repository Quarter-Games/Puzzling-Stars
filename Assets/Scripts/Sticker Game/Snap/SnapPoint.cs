using UnityEngine;

public class SnapPoint : SnapObject
{
    [SerializeField] private Transform _snapPoint;

    public override Vector3 GetSnapPosition()
    {
        return _snapPoint.position;
    }

    public override Transform GetSnapTransform()
    {
        return _snapPoint;
    }

}
