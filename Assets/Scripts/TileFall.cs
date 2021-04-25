using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFall : MonoBehaviour
{
    public GameObject hexagone;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // TODO other script and hide shadows while falling
        var position = transform.position;
        if (position.y > 0)
        {
            transform.Translate(0, 30 * -Time.deltaTime, 0);
            return;
        }

        position = new Vector3(position.x, 0, position.z);
        transform.position = position;
        hexagone.layer = 11;
        
        Destroy(this);
    }
}