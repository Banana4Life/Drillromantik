using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusScript : MonoBehaviour
{
    public float ttl = 2f;

    private float _dir;

    // Start is called before the first frame update
    void Start()
    {
        _dir = Random.Range(-1f, +1f) * 100;
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;
        transform.
        transform.Translate( (_dir + Random.Range(-0.5f,0.5f)) * Time.deltaTime, Random.Range(0f, 1f) * Time.deltaTime * 150, 0);
        if (ttl < 0)
        {
            Destroy(gameObject);
        }
    }
}
