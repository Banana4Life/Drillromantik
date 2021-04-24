using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    public float PanSpeed = 1f;
    public PostProcessVolume PostProcessVolume;

    // Update is called once per frame
    void Update()
    {
        var oldPos = transform.position;
        if (Input.GetButton("Fire2"))
        {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            transform.Translate(-dx * PanSpeed, 0, -dy * PanSpeed);
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
