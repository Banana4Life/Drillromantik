using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ResourceBarController : MonoBehaviour
    {
        public GameObject resourceStatusPrefab;

        private Dictionary<ItemType, ResourceStatus> _statusMap = new Dictionary<ItemType, ResourceStatus>();
        
        private void Start()
        {
            ItemType[] types = (ItemType[]) Enum.GetValues(typeof(ItemType));
            foreach (ItemType type in types)
            {
                var statusObject = Instantiate(resourceStatusPrefab, transform, true);
                var status = statusObject.GetComponent<ResourceStatus>();
                status.SetAvailable(1);
                _statusMap[type] = status;
            }
        }
    }
}