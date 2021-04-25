using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    public float PanSpeed = 1f;
    public float minZoomLevel = 10f;
    public float maxZoomLevel = 30f;
    public float startZoomLevel = 20f;
    public float zoomSpeed = 60f;
    public PostProcessVolume PostProcessVolume;

    private void Start()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, startZoomLevel, pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        var oldPos = transform.position;
        if (Input.GetButton("Fire2"))
        {
            float dx = Input.GetAxis("Mouse X");
            float dz = Input.GetAxis("Mouse Y");

            transform.Translate(-dx * PanSpeed, 0, -dz * PanSpeed);
        }

        var dy = Input.GetAxis("Mouse ScrollWheel");
        if (dy != 0f)
        {
            var pos = transform.position;
            pos.y = Mathf.Clamp(pos.y - dy * zoomSpeed, minZoomLevel, maxZoomLevel);
            transform.position = pos;
        }
          
        var cameraTransform = Camera.main.transform;
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var centerTileHit))
        {
            transform.position = oldPos;
        }
        else
        {
            var depthOfField = PostProcessVolume.profile.GetSetting<DepthOfField>();
            depthOfField.focusDistance.value = centerTileHit.distance;
        }
    }
}
