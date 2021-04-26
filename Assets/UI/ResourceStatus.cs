using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static Util;
using Random = UnityEngine.Random;

namespace UI
{
    public class ResourceStatus : MonoBehaviour
    {
        private Text _text;

        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponentInChildren<Text>();
        }

        public void Init(BigInteger n)
        {
            _text = GetComponentInChildren<Text>();
            UpdateText(n);
        }
        
        public void UpdateText(BigInteger n)
        {
            gameObject.SetActive(n != BigInteger.Zero);
            if (n != BigInteger.Zero && _text)
            {
                _text.text = FormatLargeNumber(n);
            }
        }
    }
}
