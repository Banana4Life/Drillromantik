using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public float panSpeed = 0.5f;
    public GameObject pointerPrefab;
    public float pointerHeight = 5f;
    private GameObject _pointer;
    public float minZoomLevel = 10f;
    public float maxZoomLevel = 30f;
    public float startZoomLevel = 20f;
    public float scrollSpeed = 10f;
    
    private float _targetZoomLevel;
    private Vector2 _targetPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _targetZoomLevel = startZoomLevel;
        _camera = GetComponentInChildren<Camera>();
        _pointer = Instantiate(pointerPrefab, transform, false);
        _pointer.SetActive(false);

        var currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, Mathf.Clamp(startZoomLevel, minZoomLevel, maxZoomLevel), currentPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        var worldPoint = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var hoveringTile = Physics.Raycast(worldPoint, out hit);
        _pointer.SetActive(hoveringTile);
        if (hoveringTile)
        {
            _pointer.transform.position = hit.collider.transform.position;
            _pointer.transform.Translate(Vector3.up * pointerHeight);
        }
        if (Input.GetButton("Fire2"))
        {
            var dx = Input.GetAxis("Mouse X");
            var dz = Input.GetAxis("Mouse Y");

            transform.Translate(-dx * panSpeed, 0, -dz * panSpeed);
        }

        var dy = Input.GetAxis("Mouse ScrollWheel");
        if (dy != 0f)
        {
            var pos = transform.position;
            pos.y = Mathf.Clamp(pos.y - dy * scrollSpeed, minZoomLevel, maxZoomLevel);
            transform.position = pos;
        }
    }
}
