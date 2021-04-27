using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    public float holding;
    public TileScript tileSelected;
    public TileScript tileHover;
    public TileMenuController tileMenuController;
    public GameObject plusPrefab;
    public AudioSource clickSource;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray worldPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        const int selectableTileLayerMask = 1 << 11;
        var hoveringTile = Physics.Raycast(worldPoint, out RaycastHit hit, Mathf.Infinity, selectableTileLayerMask);
        if (hoveringTile)
        {
            var tileScript = hit.collider.transform.parent.gameObject.GetComponent<TileScript>();
            if (tileHover != tileScript)
            {
                if (tileHover && tileHover != tileSelected)
                {
                    tileHover.UnSelectTile();
                }

                tileHover = tileScript;
                if (tileScript && tileHover != tileSelected)
                {
                    tileHover.HoverTile();
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (tileSelected)
                {
                    tileSelected.UnSelectTile();
                }

                tileSelected = tileScript;
                tileSelected.SelectTile();
                tileMenuController.TileSelected(tileScript);
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

    public void ClickSelectedTile()
    {
        clickSource.Play();
        tileSelected.ClickTile(tileMenuController.clickButton.transform);
        var text = tileMenuController.clickButton.GetComponentInChildren<Text>();
        if (text)
        {
            text.gameObject.SetActive(false);
        }
    }
}