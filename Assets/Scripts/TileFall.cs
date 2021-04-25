using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFall : MonoBehaviour
{
    public GameObject hexagone;
    public const int StartHeight = 50;
    public const float Duration = 1f;
    public float currenDuration = 0f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        currenDuration += Time.deltaTime;
        if (currenDuration < Duration)
        {
            float y = Mathf.Lerp(StartHeight, 0, EaseOut(currenDuration / Duration));
            transform.position = new Vector3(position.x, y, position.z);
            return;
        }

        transform.position = new Vector3(position.x, 0, position.z);
        hexagone.layer = 11;

        Destroy(this);
    }

    private static float Flip(float t)
    {
        return 1 - t;
    }

    private static float EaseOut(float t)
    {
        return Flip(Mathf.Pow(Flip(t), 3));
    }
}