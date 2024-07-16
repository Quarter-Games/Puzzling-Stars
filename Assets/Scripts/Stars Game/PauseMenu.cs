using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject GameEndWindow;
    [SerializeField] Camera MainCamera;
    [SerializeField] Vector3 FinalCameraPosition;
    [SerializeField] float Timer;
    [SerializeField] GameObject Album;

    private void OnEnable()
    {
        LevelManager.OnLevelComplete += OnLevelComplete;
        foreach (var constell in GetComponentsInChildren<Constellation>(true))
        {
            constell.CopiedFrom.OnConstellationComplete += constell.LeaveOnlyImage;
        }
    }

    private void OnLevelComplete()
    {
        //GameEndWindow.SetActive(true);
        Album.SetActive(true);
        StartCoroutine(MoveCameraDownAndChangeToPerspective());
    }

    IEnumerator MoveCameraDownAndChangeToPerspective()
    {
        float t = 0;
        Vector3 startPos = MainCamera.transform.position;
        while (t < Timer)
        {
            t += Time.fixedDeltaTime;
            MainCamera.transform.position = Vector3.Lerp(startPos, FinalCameraPosition, t / Timer);
            if (MainCamera.orthographic && t / Timer >= 0.5f)
            {
                MainCamera.orthographic = false;
                MainCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
            }
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(RotateAlbum());
    }
    IEnumerator RotateAlbum()
    {
        float t = 0;
        Quaternion startRotation = Album.transform.rotation;
        float StartFov = MainCamera.fieldOfView;
        List<GameObject> images = GetComponentsInChildren<Constellation>(true).Select(x => x.transform.GetChild(0).gameObject).ToList();
        List<Image> stickers = Album.GetComponentsInChildren<Image>().Where(x => x.CompareTag("Sticker")).ToList();
        Dictionary<RectTransform, RectTransform> stickersToImages = new Dictionary<RectTransform, RectTransform>();
        var albumCan = Album.GetComponentInChildren<Canvas>();
        var canvas = images[0].GetComponentInParent<Canvas>();
        canvas.transform.position = albumCan.transform.position;
        canvas.transform.position -= canvas.transform.forward;
        //canvas.transform.rotation = albumCan.transform.rotation;
        canvas.transform.localScale = albumCan.transform.lossyScale;

        foreach (var sticker in stickers)
        {
            var temp = images.Find(x => x.GetComponent<Image>().sprite == sticker.sprite);
            stickersToImages.Add(sticker.GetComponent<RectTransform>(), temp.GetComponent<RectTransform>());
            temp.transform.SetParent(sticker.transform.parent,true);
            temp.AddComponent<LayoutElement>().ignoreLayout = true;
        }
        float timer2 = Timer / 2;
        while (t < timer2)
        {
            t += Time.fixedDeltaTime;
            Album.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t / timer2);
            MainCamera.fieldOfView = Mathf.Lerp(StartFov, 25, t / timer2);
            if (t / timer2 < 0.4f)
            {
                foreach (var pair in stickersToImages)
                {
                    pair.Value.sizeDelta = Vector2.Lerp(pair.Value.sizeDelta, pair.Key.sizeDelta, t / timer2);
                    pair.Value.rotation = Quaternion.Lerp(pair.Value.rotation, pair.Key.rotation, t / timer2);
                    pair.Value.position = Vector3.Lerp(pair.Value.position, pair.Key.position, t / timer2);
                }
            }
            else
            {
                foreach (var pair in stickersToImages)
                {
                    pair.Value.sizeDelta = Vector2.Lerp(pair.Value.sizeDelta, pair.Key.sizeDelta, t / timer2);
                    pair.Value.rotation = Quaternion.Lerp(pair.Value.rotation, pair.Key.rotation, t / timer2);
                    pair.Value.position = Vector3.Lerp(pair.Value.position, pair.Key.position, t / timer2);
                    pair.Value.localScale = Vector3.Lerp(pair.Value.localScale, pair.Key.localScale, t / timer2);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        foreach (var sticker in stickers)
        {
            sticker.enabled = true;
        }
        foreach (var image in images)
        {
            DestroyImmediate(image.gameObject);
        }
    }
    public void OnMainMenuButtonClicked()
    {
        GameEndWindow.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
    private void OnDisable()
    {
        LevelManager.OnLevelComplete -= OnLevelComplete;
    }
}
