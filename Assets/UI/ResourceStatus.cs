using UnityEngine;
using UnityEngine.UI;

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

        public void SetAvailable(decimal n)
        {
            _text.text = $"{n}";
        }
    }
}
