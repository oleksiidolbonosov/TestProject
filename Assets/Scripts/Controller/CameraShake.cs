using System.Collections;
using UnityEngine;

namespace Game.Controller
{
    public class CameraShake : MonoBehaviour
    {
        private static CameraShake _instance;

        public static CameraShake Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CameraShake>();

                    if (_instance == null)
                    {
                        var obj = new GameObject("CameraShake");
                        _instance = obj.AddComponent<CameraShake>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private float _defaultDuration = 0.2f;
        [SerializeField] private float _defaultMagnitude = 0.7f;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Shake(Vector3 direction)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine(direction, _defaultDuration, _defaultMagnitude));
        }

        private IEnumerator ShakeCoroutine(Vector3 direction, float duration, float magnitude)
        {
            var originalPosition = transform.localPosition;
            var elapsed = 0.0f;
            var halfDuration = duration / 2f;

            while (elapsed < duration)
            {
                var time = elapsed / duration;
                var amplitude = Mathf.Lerp(magnitude, 0, time / halfDuration);

                if (float.IsNaN(amplitude) || float.IsInfinity(amplitude))
                {
                    amplitude = 0f;
                }

                var offset = GenerateShakeOffset(amplitude);

                if (float.IsNaN(offset.x) || float.IsNaN(offset.y) || float.IsNaN(offset.z))
                {
                    offset = Vector3.zero;
                }

                transform.localPosition = originalPosition + Vector3.Scale(offset, -direction.normalized);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPosition;
        }

        private Vector3 GenerateShakeOffset(float magnitude)
        {
            if (float.IsNaN(magnitude) || float.IsInfinity(magnitude))
            {
                magnitude = 0f;
            }

            var x = Mathf.PerlinNoise(Time.time * magnitude, 0.0f) * 2f - 1f;
            var y = Mathf.PerlinNoise(0.0f, Time.time * magnitude) * 2f - 1f;
            var z = Mathf.Sin(Time.time * Mathf.PI * 2f / magnitude) * magnitude;

            if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z))
            {
                return Vector3.zero;
            }

            return new Vector3(x, y, z) * magnitude;
        }
    }
}
