using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControll : MonoBehaviour
{
    public static event Action<LevelSettings> OnLevelLoad;
    private bool isRotating = false;
    [SerializeField] Camera _camera;
    [SerializeField] float ZoomedInFieldOfView;
    [SerializeField] float LoadingTime;
    [SerializeField] Image FadeInPanel;
    [SerializeField] GameObject Book;

    private void OnEnable()
    {
        MainMenuConstallation.OnConstallationClick += ZoomIn;
    }
    private void OnDisable()
    {
        MainMenuConstallation.OnConstallationClick -= ZoomIn;
    }
    private void ZoomIn(MainMenuConstallation constallation)
    {
        if (isRotating) return;
        StartCoroutine(ZoomIn_Coroutine(constallation));
    }

    private IEnumerator ZoomIn_Coroutine(MainMenuConstallation constallation)
    {
        isRotating = true;
        var loadingOperation = SceneManager.LoadSceneAsync(constallation.settings.SceneName);
        loadingOperation.allowSceneActivation = false;
        Vector3 StartLookAtPoint = _camera.transform.position + _camera.transform.forward * 30;
        float timer = 0;
        while (timer < LoadingTime)
        {
            timer += Time.deltaTime;
            _camera.fieldOfView = Mathf.Lerp(60, ZoomedInFieldOfView, timer/LoadingTime);
            _camera.transform.LookAt(Vector3.Lerp(StartLookAtPoint, constallation.ZoomInPoint.position, timer / LoadingTime));
            FadeInPanel.color = new Color(0, 0, 0, timer / LoadingTime/1.5f);
            yield return new WaitForEndOfFrame();
        }
        var scene = SceneManager.GetActiveScene();
        loadingOperation.allowSceneActivation = true;
        isRotating = false;
        OnLevelLoad?.Invoke(constallation.settings);
    }

    public bool Rotate(MainMenuRotationButton mainMenuRotation)
    {
        if (isRotating) return false;
        StartCoroutine(Rotate_Coroutine(mainMenuRotation));
        return true;
    }
    public IEnumerator Rotate_Coroutine(MainMenuRotationButton mainMenuRotation)
    {
        isRotating = true;
        var timer = mainMenuRotation.Duration;
        var startRotation = _camera.transform.localRotation;
        Space space = mainMenuRotation.Vertical ? Space.Self : Space.World;
        while (timer > 0)
        {
            timer -= mainMenuRotation.timeTick;
            _camera.transform.Rotate(mainMenuRotation.Axis, mainMenuRotation.Angle * mainMenuRotation.timeTick / mainMenuRotation.Duration, space);

            yield return new WaitForSeconds(mainMenuRotation.timeTick);
        }
        _camera.transform.rotation = startRotation;
        _camera.transform.Rotate(mainMenuRotation.Axis, mainMenuRotation.Angle, space);
        isRotating = false;
    }
}
