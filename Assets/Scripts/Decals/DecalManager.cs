using UnityEngine;
using System.Collections;
using Game.Controller;
using System;

namespace Game.Utils
{
    public class DecalManager : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private int _renderTextureWidth = 1024;
        [SerializeField] private int _renderTextureHeight = 256;
        [SerializeField] private float _cameraDistance = 10f;
        
        private static string _decalLayerName = "Decal";

        private RenderTexture _renderTexture;
        private GameObject _currentDecal;
        private Camera _renderCamera;
        private GameObject _decalPrefab;

        private void Awake()
        {
            _renderCamera = transform.GetComponentInChildren<Camera>();
            GetComponent<Renderer>().material = new Material(_material);
            CreateRenderTextureAndCamera();

            ProjectileController.OnAddDecal += CreateDecal;
        }

        private void Start()
        {
            _currentDecal = Instantiate(_decalPrefab);
            _currentDecal.SetActive(false);
            _currentDecal.layer = LayerMask.NameToLayer(_decalLayerName);
        }

        private void CreateRenderTextureAndCamera()
        {
            _renderTexture = new RenderTexture(_renderTextureWidth, _renderTextureHeight, 0, RenderTextureFormat.ARGB32);
            _renderTexture.Create();

            _renderCamera.enabled = false;
            _renderCamera.targetTexture = _renderTexture;

            InitializeRenderTexture();
        }

        private void InitializeRenderTexture()
        {
            if (_renderTexture == null)
            {
                Debug.LogError("RenderTexture is not assigned.");
                return;
            }

            var whiteTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.ARGB32, false);

            var pixels = new Color[whiteTexture.width * whiteTexture.height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }
            whiteTexture.SetPixels(pixels);
            whiteTexture.Apply();

            RenderTexture.active = _renderTexture;
            GL.Clear(true, true, Color.white);

            Graphics.Blit(whiteTexture, _renderTexture);

            RenderTexture.active = null;
            Destroy(whiteTexture);
            AssignRenderTextureToMaterial();
            SetupCameraForObject();
        }

        public void CreateDecal(Vector3 position, Vector3 normal, GameObject hitObject)
        {
            if (_currentDecal != null)
            {
                var normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
                var additionalRotation = Quaternion.Euler(90, 0, 0);
                var combinedRotation = normalRotation * additionalRotation;

                _currentDecal.transform.position = position;
                _currentDecal.transform.rotation = combinedRotation;
                _currentDecal.SetActive(true);

                StartCoroutine(RenderAndApplyDecal());
            }
        }

        private void SetupCameraForObject()
        {
            var renderer = transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                var bounds = renderer.bounds;
                var objectCenter = bounds.center;
                _renderCamera.transform.position = objectCenter - _cameraDistance * _renderCamera.transform.forward;
                _renderCamera.transform.LookAt(objectCenter);
            }
        }

        private IEnumerator RenderAndApplyDecal()
        {
            yield return new WaitForEndOfFrame();
            _renderCamera.Render();
            AssignRenderTextureToMaterial();

            _currentDecal.SetActive(false);
        }

        private void AssignRenderTextureToMaterial()
        {
            var renderer = transform.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.mainTexture = _renderTexture;
            }
        }

        private void OnDestroy()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
            }

            if (_renderCamera != null)
            {
                Destroy(_renderCamera.gameObject);
            }

            if (_currentDecal != null)
            {
                Destroy(_currentDecal);
            }
        }

        public void Initialize(GameObject decalPrefab)
        {
            _decalPrefab = decalPrefab;
        }
    }
}
