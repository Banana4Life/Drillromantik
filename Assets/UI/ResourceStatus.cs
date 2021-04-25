using System;
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
        private decimal _available = Decimal.Zero;

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
            if (_text != null)
            {
                _text.text = $"{_available}";
            }
            gameObject.SetActive(_available != Decimal.Zero);
        }

        public void SetAvailable(decimal n)
        {
            _available = n;
            updateText();
        }
    }
}
