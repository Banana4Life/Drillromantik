using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 5;
    public Vector3 axis = Vector3.up;

    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
    }
}
