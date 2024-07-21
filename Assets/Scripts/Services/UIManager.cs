using UnityEngine;
using UnityEngine.UI;

namespace Game.Services
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private Slider _powerSlider;
        [SerializeField] private Text _powerText;
        [SerializeField] private float _minPower = 1f;
        [SerializeField] private float _maxPower = 100f;
        [SerializeField] private float _changeSpeed = 10f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _powerSlider.value += _changeSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _powerSlider.value -= _changeSpeed * Time.deltaTime;
            }

            _powerSlider.value = Mathf.Clamp(_powerSlider.value, _minPower, _maxPower);
            _powerText.text = ((int)_powerSlider.value).ToString();
        }

        public float GetFirePower()
        {
            return _powerSlider.value;
        }
    }
}
