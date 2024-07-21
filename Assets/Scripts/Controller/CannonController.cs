using Game.Factory;
using UnityEngine;
using System.Collections;
using Game.Model;
using Game.Services;

namespace Game.Controller
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float _fireRate = 1f;

        private float _nextFireTime;
        private ProjectileFactory _projectileFactory;
        private ProjectileModel _projectileModel;
        private CameraShake _cameraShake;
        private ITrajectoryVisualizer _trajectoryVisualizer;
        private IUIManager _uiManager;
        private GameObject _projectilePrefab;
        private Transform _firePoint;

        private void Awake()
        {
            _projectileModel = new ProjectileModel
            {
                Position = transform.position,
            };

            _cameraShake = Camera.main.GetComponent<CameraShake>();
            _firePoint = GameObject.Find("FirePoint").transform;
        }

        private void Start()
        {
            _projectileFactory = new ProjectileFactory(_projectilePrefab, ServiceLocator.GetMeshGeneratorService());
            _trajectoryVisualizer = ServiceLocator.GetTrajectoryVisualizer();
            _trajectoryVisualizer.Initialize(_projectileModel, _firePoint);

            _uiManager = ServiceLocator.GetUIManager();
        }

        public void Fire()
        {
            if (Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + _fireRate;

                var projectileInstance = _projectileFactory.CreateProjectile(_firePoint.position, _firePoint.rotation);
                projectileInstance.Initialize(_projectileFactory, _firePoint.TransformDirection(Vector3.forward) * _uiManager.GetFirePower(), _firePoint.rotation);
                var fireDirection = _firePoint.TransformDirection(Vector3.forward);
                var cameraRelativeDirection = Camera.main.transform.InverseTransformDirection(fireDirection);
                _cameraShake.Shake(cameraRelativeDirection);
                StartCoroutine(FireAnimation());
            }
        }

        private IEnumerator FireAnimation()
        {
            var originalPosition = transform.position;
            var recoilDirection = -transform.up;
            var recoilOffset = recoilDirection * 0.1f;

            var elapsedTime = 0f;
            var duration = 0.05f;

            while (elapsedTime < duration / 2f)
            {
                transform.position = Vector3.Lerp(originalPosition, originalPosition + recoilOffset, elapsedTime / (duration / 2f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition + recoilOffset;
            elapsedTime = 0f;

            while (elapsedTime < duration / 2f)
            {
                transform.position = Vector3.Lerp(originalPosition + recoilOffset, originalPosition, elapsedTime / (duration / 2f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }

        private void OnDisable()
        {
            _projectileFactory.ClearPool();
        }

        public void Initialize(GameObject projectilePrefab)
        {
            _projectilePrefab = projectilePrefab;
        }
    }
}
