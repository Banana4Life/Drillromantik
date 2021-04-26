using System;
using UnityEngine;

namespace UI
{
    public class DepthIndicatorController : MonoBehaviour
    {
        public GameObject needle;

        private RectTransform _myTransform;
        private RectTransform _needleTransform;

        private void Start()
        {
            _myTransform = GetComponent<RectTransform>();
            _needleTransform = needle.GetComponent<RectTransform>();
        }

        private void Update()
        {
            _needleTransform.Translate(Vector3.down * Time.deltaTime);
        }
    }
}