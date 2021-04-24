using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public float PanSpeed = 1f;
    public GameObject PointerPrefab;
    private GameObject _pointer;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _pointer = Instantiate(PointerPrefab, transform, false);
        _pointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray worldPoint = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var hoveringTile = Physics.Raycast(worldPoint, out hit);
        _pointer.SetActive(hoveringTile);
        if (hoveringTile)
        {
            _pointer.transform.position = hit.collider.transform.position;
            _pointer.transform.Translate(Vector3.up * 5);
        }
        if (Input.GetButton("Fire2"))
        {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            transform.Translate(-dx * PanSpeed, 0, -dy * PanSpeed);
        }
    }
}
