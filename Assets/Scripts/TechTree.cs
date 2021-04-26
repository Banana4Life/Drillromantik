using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public Structure[] Structures;
    public ItemTypeTexture[] Textures;
}

[Serializable]
public class ItemTypeTexture
{
    public ItemType type;
    public Texture tex;
}
