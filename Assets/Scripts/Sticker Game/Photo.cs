using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    [SerializeField] List<Sticker> StickersToFinish;
    [SerializeField] List<SnapObject> SnapObjects;
    public Image MovableSticker;
    public static bool attempted;
    private void OnEnable()
    {
        Sticker.DragImageStarted += Sticker_DragImageStarted;
        Sticker.StickerIsSnapped += FinishCheck;
        attempted = false;
    }

    private void FinishCheck()
    {
        Debug.Log("Snapped");
        if (SnapObjects.Count(x => x.SnappedSticker != null) == StickersToFinish.Count)
        {
            Debug.Log("Every Sticker is placed");
            attempted = true;
            if (SnapObjects.Where(x=>x.SnappedSticker!=null).All(x => x.SnappedSticker.RightPlace == x))
            {
                Debug.Log("Every Sticker is placed correctly");
            }
        }
    }

    private void OnDisable()
    {
        Sticker.DragImageStarted -= Sticker_DragImageStarted;
        Sticker.StickerIsSnapped -= FinishCheck;
    }

    private Image Sticker_DragImageStarted()
    {
        return MovableSticker;
    }

}
