using UnityEngine;
using UnityEngine.EventSystems;

abstract public class SnapObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static SnapObject CurrentSnapObject;
    [SerializeField] protected Collider snapCollider;
    Sticker SnappedSticker;


    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        if (Sticker.CurrentDraged == null) return;
        CurrentSnapObject = this;
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        if (Sticker.CurrentDraged == null) return;
        if (CurrentSnapObject != this)
        {
            //Debug.LogError("Snap Colliders Are overlapping!");
            return;
        }
        CurrentSnapObject = null;

    }
    abstract public Transform GetSnapTransform();
    abstract public Vector3 GetSnapPosition();
    virtual public void Snap(Sticker sticker)
    {
        snapCollider.enabled = false;
        SnappedSticker = sticker;
    }
    virtual public void UnSnap()
    {
        SnappedSticker = null;
        snapCollider.enabled = true;
        if (CurrentSnapObject == this)
        {
            CurrentSnapObject = null;
        }
    }
}
