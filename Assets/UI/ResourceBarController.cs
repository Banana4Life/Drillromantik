using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
                status.Available = 1;
                _statusMap[type] = status;
            }

            StartCoroutine(IncreaseResources());
        }

        IEnumerator IncreaseResources()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                var factor = BigInteger.One * 10;
                foreach (var keyValuePair in _statusMap)
                {
                    keyValuePair.Value.Available *= factor;
                }
            }
        }
    }
}