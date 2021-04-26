using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class FloatyScript : MonoBehaviour
{
    public float ttl = 2f;

    public GameObject resourcePrefab;

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

    public void Init(string floatyName, Resources resources, float ttl, Color color)
    {
        this.ttl = ttl;

        GetComponent<Text>().text = floatyName;
        GetComponent<Text>().color = color;

        foreach (var item in resources.Items)
        {
            var resObj = Instantiate(resourcePrefab, GetComponentInChildren<VerticalLayoutGroup>().transform);
            
            var status = resObj.GetComponent<ResourceStatus>();
            status.Init(item.Value);
            foreach (var techTreeTexture in Global.FindTechTree().Textures)
            {
                if (techTreeTexture.type == item.Key)
                {
                    status.GetComponentInChildren<RawImage>().texture = techTreeTexture.tex;
                    break;
                }
            }
        }
        
    }
}
