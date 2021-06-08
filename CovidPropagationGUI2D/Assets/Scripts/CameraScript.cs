using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public Button btnZoomIn;
    public Button btnZoomOut;

    private Vector3 defaultPosition;
    private Vector3 dragOrigin;
    private Vector3 cameraMinPosition;
    private Vector3 cameraMaxPosition;

    private float zoomStep = 1;
    private float minSize = 1;
    private float maxSize = 10;

    // Start is called before the first frame update
    void Start()
    {
        btnZoomIn.onClick.AddListener(ZoomIn);
        btnZoomOut.onClick.AddListener(ZoomOut);
        defaultPosition = cam.transform.position;
        cameraMinPosition = new Vector3(-8, -5, 0);
        cameraMaxPosition = new Vector3(8 ,5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = difference;

            if (newPosition.x + cam.transform.position.x < cameraMinPosition.x || newPosition.x + cam.transform.position.x > cameraMaxPosition.x)
            {
                difference.x = 0;
            }

            if (newPosition.y + cam.transform.position.y < cameraMinPosition.y || newPosition.y + cam.transform.position.y > cameraMaxPosition.y)
            {
                difference.y = 0;
            }

            cam.transform.position += difference;
        }

        if (Input.GetMouseButtonDown(1))
        {
            cam.transform.position = defaultPosition;
        }

    }

    private void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }

    private void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }
}
