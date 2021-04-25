using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public Structure[] Structures;
    public ItemTypeTexture[] Textures;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class ItemTypeTexture
{
    public ItemType type;
    public Texture tex;
}