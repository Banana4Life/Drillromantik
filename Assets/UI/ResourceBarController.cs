using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBarController : MonoBehaviour
    {
        public GameObject resourceStatusPrefab;
        private TechTree _techTree;

        public bool cheats;

        private Dictionary<ItemType, ResourceStatus> _statusMap = new Dictionary<ItemType, ResourceStatus>();
        
        private void Start()
        {
            _techTree = Global.FindTechTree();
            ItemType[] types = (ItemType[]) Enum.GetValues(typeof(ItemType));
            foreach (ItemType type in types)
            {
                var statusObject = Instantiate(resourceStatusPrefab, transform, true);
                var status = statusObject.GetComponent<ResourceStatus>();
                _statusMap[type] = status;
                
                //status.UpdateText(BigInteger.Zero);
                status.UpdateText(300);
            }
            foreach (var techTreeTexture in _techTree.Textures)
            {
                _statusMap[techTreeTexture.type].GetComponentInChildren<RawImage>().texture = techTreeTexture.tex;
            }
        }

        private void Update()
        {
            if (cheats)
            {
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.WOOD});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.STONE});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.COPPER});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.COAL});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.GOLD});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.CHARCOAL});
                Global.Resources.Add(new Item { quantity = 10, type = ItemType.IRON});    
            }
            
            foreach (var status in _statusMap)
            {
                status.Value.UpdateText(Global.Resources.Items.GetOrElse(status.Key));
            }
        }
    }
}