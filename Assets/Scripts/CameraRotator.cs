using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform center;

    private Vector3 prevPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            prevPos = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 direction = prevPos - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = center.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180.0f);
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.z * 180.0f, Space.World);
            cam.transform.Translate(new Vector3(0, 0, -10));

            prevPos = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
