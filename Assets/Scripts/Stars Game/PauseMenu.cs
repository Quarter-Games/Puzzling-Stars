using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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
        while (t < (Timer) / 2)
        {
            t += Time.fixedDeltaTime;
            Album.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t / (Timer / 2));
            MainCamera.fieldOfView = Mathf.Lerp(StartFov, 25, t / (Timer / 2));
            yield return new WaitForFixedUpdate();
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
