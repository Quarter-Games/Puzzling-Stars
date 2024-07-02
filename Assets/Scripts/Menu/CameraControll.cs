using System.Collections;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    
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
}
