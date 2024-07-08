using System.Collections;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private bool isRotating = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(RotateCamera());
        }
    }
    private IEnumerator RotateCamera()
    {
        while (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
            transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            yield return null;
        }
    }
    public void Rotate(MainMenuRotationButton mainMenuRotation)
    {
        if (isRotating) return;
        StartCoroutine(Rotate_Coroutine(mainMenuRotation));
    }
    public IEnumerator Rotate_Coroutine(MainMenuRotationButton mainMenuRotation)
    {
        isRotating = true;
        var timer = mainMenuRotation.Duration;
        while (timer >= 0)
        {
            timer -= mainMenuRotation.timeTick;
            mainMenuRotation._camera.transform.Rotate(mainMenuRotation.Axis, mainMenuRotation.Angle * mainMenuRotation.timeTick / mainMenuRotation.Duration, Space.World);

            yield return new WaitForSeconds(mainMenuRotation.timeTick);
        }
        isRotating = false;
    }
}
