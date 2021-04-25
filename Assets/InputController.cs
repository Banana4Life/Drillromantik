using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameObject _pointer;
    public GameObject PointerPrefab;
    public float holding;

    
    // Start is called before the first frame update
    void Start()
    {
        _pointer = Instantiate(PointerPrefab, transform, false);
        _pointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray worldPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var hoveringTile = Physics.Raycast(worldPoint, out hit);
        _pointer.SetActive(hoveringTile);
        if (hoveringTile)
        {
            
            _pointer.transform.position = hit.collider.transform.position;
            _pointer.transform.Translate(Vector3.up * 5);
            
            if (Input.GetButtonDown("Fire1"))
            {
                hit.collider.transform.parent.gameObject.GetComponent<TileScript>().ClickTile();
            }

            if (Input.GetButton("Fire1"))
            {
                holding += Time.deltaTime;
                if (holding > 1)
                {
                    Debug.Log("Opening Radial Menu soon (tm)");
                }
            }
            else
            {
                holding = 0;
            }
        }
        
        
    }
}
