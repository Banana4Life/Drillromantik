using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBarController : MonoBehaviour
    {
        public GameObject resourceStatusPrefab;
        public TechTree TechTree;

        private Dictionary<ItemType, ResourceStatus> _statusMap = new Dictionary<ItemType, ResourceStatus>();
        
        private void Start()
        {
            ItemType[] types = (ItemType[]) Enum.GetValues(typeof(ItemType));
            foreach (ItemType type in types)
            {
                var statusObject = Instantiate(resourceStatusPrefab, transform, true);
                var status = statusObject.GetComponent<ResourceStatus>();
                _statusMap[type] = status;
                
                //status.UpdateText(BigInteger.Zero);
                status.UpdateText(300);
            }
            foreach (var techTreeTexture in TechTree.Textures)
            {
                _statusMap[techTreeTexture.type].GetComponentInChildren<RawImage>().texture = techTreeTexture.tex;
            }
        }

        private void Update()
        {
            foreach (var status in _statusMap)
            {
                status.Value.UpdateText(Global.Resources.Items.GetOrElse(status.Key));
            }
        }
    }
}