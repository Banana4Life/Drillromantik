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
        private Image _icon;


        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponentInChildren<Text>();
            _icon = GetComponentInChildren<Image>();

            _icon.color = Random.ColorHSV();
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
