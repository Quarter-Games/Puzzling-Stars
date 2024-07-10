using UnityEngine;
using UnityEngine.EventSystems;

abstract public class SnapObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static SnapObject CurrentSnapObject;
    [SerializeField] protected Collider snapCollider;
    public Sticker RightSticker;
    [SerializeField] Sticker SnappedSticker;


    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        if (SnappedSticker != null) return;
        if (Sticker.CurrentDraged == null) return;
        if (CurrentSnapObject != null) return;
        CurrentSnapObject = this;
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        if (SnappedSticker != null) return;
        if (Sticker.CurrentDraged == null) return;
        if (CurrentSnapObject != this) return;
        CurrentSnapObject = null;

    }
    abstract public Transform GetSnapTransform();
    abstract public Vector3 GetSnapPosition();
    virtual public void Snap(Sticker sticker)
    {
        snapCollider.enabled = false;
        SnappedSticker = sticker;
        enabled = false;
        CurrentSnapObject = null;
    }
    virtual public void UnSnap()
    {
        SnappedSticker = null;
        snapCollider.enabled = true;
        if (CurrentSnapObject == this)
        {
            CurrentSnapObject = null;
        }
        enabled = true;
    }
    private void OnDrawGizmos()
    {
        if (RightSticker == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, RightSticker.transform.position);
    }
}
