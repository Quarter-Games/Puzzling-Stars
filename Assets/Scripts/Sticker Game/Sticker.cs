using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sticker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static event Action StickerIsSnapped;
    public static event Func<Image> DragImageStarted;
    [SerializeField] Image Sprite;
    public SnapObject RightPlace;
    public Material WrongMaterial;
    public static Sticker CurrentDraged { get; private set; }
    Image DraggedImage;
    public Sticker CoppiedFrom;
    public SnapObject SnappedTo;
    private void Awake()
    {
        StickerIsSnapped += UpdateComplition;
    }
    private void OnDestroy()
    {
        StickerIsSnapped -= UpdateComplition;
    }

    private void UpdateComplition()
    {
        if (Photo.attempted)
        {
            if (RightPlace == SnappedTo)
            {
                ShowComplition();
            }
            else
            {
                ShowIncomplition();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check
        if (CurrentDraged != null) return;
        //Getting Grabbing Image
        DraggedImage = DragImageStarted?.Invoke();
        if (DraggedImage == null)
        {
            return;
        }
        CurrentDraged = this;
        //Setting it Up
        DraggedImage.gameObject.SetActive(true);
        DraggedImage.sprite = Sprite.sprite;
        DraggedImage.SetNativeSize();
        //Moving Image
        DraggedImage.transform.position = transform.position;
        //Disianbling the image
        Sprite.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CurrentDraged != this) return;
        DraggedImage.transform.position = eventData.pointerCurrentRaycast.worldPosition;
        DraggedImage.transform.position -= DraggedImage.transform.forward / 10;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CurrentDraged != this) return;
        if (SnapObject.CurrentSnapObject != null && SnapObject.CurrentSnapObject != SnappedTo)
        {
            var copy = Instantiate(this);
            copy.Snap(this, SnapObject.CurrentSnapObject);
        }
        else
        {
            Sprite.enabled = true;
        }
        DraggedImage.gameObject.SetActive(false);
        DraggedImage = null;
        CurrentDraged = null;
    }
    public void Snap(Sticker Coppied, SnapObject to)
    {
        transform.rotation = Coppied.DraggedImage.transform.rotation;
        SnappedTo = to;
        transform.SetParent(SnappedTo.GetSnapTransform());
        transform.position = SnappedTo.GetSnapPosition();
        transform.localScale = Vector3.one;
        Sprite.enabled = true;
        SnappedTo.Snap(this);
        if (Coppied.CoppiedFrom == null) CoppiedFrom = Coppied;
        else
        {
            Coppied.SnappedTo.UnSnap();
            CoppiedFrom = Coppied.CoppiedFrom;
            Destroy(Coppied.gameObject);
            Coppied.enabled = false;
        }
        StickerIsSnapped?.Invoke();
    }
    public void ShowComplition()
    {
        Sprite.material = null;
    }
    public void ShowIncomplition()
    {
        Sprite.material = WrongMaterial;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (CoppiedFrom != null)
            {
                CoppiedFrom.Sprite.enabled = true;
                CoppiedFrom.enabled = true;
                SnappedTo.UnSnap();
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (RightPlace == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, RightPlace.transform.position);
    }
}
