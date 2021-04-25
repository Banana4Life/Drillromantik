using System;
using System.Numerics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class ResourceStatus : MonoBehaviour
    {
        [CanBeNull] private Text _text;
        private Image _icon;
        private ItemType? Type = null;
        private BigInteger _available = BigInteger.Zero;

        private String[] _unitSuffixes = {"", " K", " M", " G", " T", " P", " E", " Z", " Y"};

        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponentInChildren<Text>();
            _icon = GetComponentInChildren<Image>();

            _icon.color = Random.ColorHSV();
            updateText();
        }
        
        private void updateText()
        {
            if (_text)
            {
                
                var thousands = Math.Min((int) (Math.Floor(Math.Round(BigInteger.Log10(_available))) / 3d), _unitSuffixes.Length - 1);
                var significant = _available / BigInteger.Pow(BigInteger.One * 10,  3 * thousands);
                String suffix = _unitSuffixes[thousands];
                _text.text = $"{significant}{suffix}";
            }

            gameObject.SetActive(_available != BigInteger.Zero);
        }

        public BigInteger Available
        {
            get => _available;
            set
            {
                _available = value;
                updateText();
            }
        }
    }
}
