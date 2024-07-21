using UnityEngine;

namespace Game.Controller
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity = 2.0f;
        [SerializeField] private float _minVerticalAngle = 70f;
        [SerializeField] private float _maxVerticalAngle = 90f;

        private float _rotation = 0f;
        private CannonController _cannonController;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cannonController = GetComponentInChildren<CannonController>();
        }

        private void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _rotation -= mouseY;
            _rotation = Mathf.Clamp(_rotation, _minVerticalAngle, _maxVerticalAngle);

            transform.Rotate(Vector3.up * mouseX);
            _cannonController.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);

            if (Input.GetButtonDown("Fire1"))
            {
                _cannonController.Fire();
            }
        }
    }
}
