using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureController : MonoBehaviour
{
    void Start()
    {
        foreach (var child in GetComponentsInChildren<Camera>())
        {
            child.Render();
        }
    }
}
