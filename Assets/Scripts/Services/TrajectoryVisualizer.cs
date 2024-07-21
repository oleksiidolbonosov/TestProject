using Game.Model;
using Game.Services;
using UnityEngine;

namespace Game.Trajectory
{
    public class TrajectoryVisualizer : MonoBehaviour, ITrajectoryVisualizer
    {
        private IUIManager _uiManager;
        private ProjectileModel _projectileModel;
        private Transform _firePoint;
        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (_projectileModel == null)
            {
                Debug.LogError("ProjectileModel is not assigned.");
                return;
            }

            _uiManager = ServiceLocator.GetUIManager();

            DrawTrajectory();
        }

        private void Update()
        {
            if (_projectileModel == null || _uiManager == null)
            {
                return;
            }

            _projectileModel.Velocity = _firePoint.TransformDirection(Vector3.forward) * _uiManager.GetFirePower();

            DrawTrajectory();
        }

        public void DrawTrajectory()
        {
            if (_projectileModel == null)
                return;

            int steps = Mathf.CeilToInt(_projectileModel.Velocity.magnitude / Time.deltaTime) + 1;
            Vector3[] points = new Vector3[steps];

            ProjectileModel tempModel = new ProjectileModel
            {
                Position = _projectileModel.Position,
                Velocity = _projectileModel.Velocity,
                Gravity = _projectileModel.Gravity
            };

            TrajectoryStrategy trajectoryStrategy = new TrajectoryStrategy();

            for (int i = 0; i < steps; i++)
            {
                points[i] = tempModel.Position;
                trajectoryStrategy.CalculateTrajectory(tempModel, Time.deltaTime);
            }

            _lineRenderer.positionCount = steps;
            _lineRenderer.SetPositions(points);
        }

        public void Initialize(ProjectileModel projectileModel, Transform firePoint)
        {
            _projectileModel = projectileModel;
            _firePoint = firePoint;
        }
    }
}
