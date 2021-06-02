using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Button btnZoomIn;
    public Button btnZoomOut;
    private Vector3 previousPosition;
    private Vector3 centrer;

    float btnIncrement = 5;
    float minFov = 40f;
    float maxFov = 90f;
    float sensitivity = -10f;
    // Start is called before the first frame update
    void Start()
    {
        btnZoomIn.onClick.AddListener(ZoomIn);
        btnZoomOut.onClick.AddListener(ZoomOut);

        previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);

        centrer = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = centrer;

            cam.transform.Rotate(new Vector3(1,0,0), direction.y * 180);
            cam.transform.Rotate(new Vector3(0,1,0), -direction.x * 180, Space.World);
            cam.transform.Translate(new Vector3(0, 0, -10));

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public void SetCenter(Vector3 center)
    {
        this.centrer = center;
    }

    private void ZoomIn()
    {
        float fov = Camera.main.fieldOfView - btnIncrement;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    private void ZoomOut()
    {
        float fov = Camera.main.fieldOfView + btnIncrement;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}
