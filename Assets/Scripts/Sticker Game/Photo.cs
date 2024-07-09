using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    public Image MovableSticker;
    private void OnEnable()
    {
        Sticker.DragImageStarted += Sticker_DragImageStarted;
    }
    private void OnDisable()
    {
        Sticker.DragImageStarted -= Sticker_DragImageStarted;
    }

    private Image Sticker_DragImageStarted()
    {
        return MovableSticker;
    }

}
