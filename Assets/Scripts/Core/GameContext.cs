using UnityEngine;
using Game.Strategy;
using Game.Services;
using Game.Trajectory;
using Game.Utils;
using Game.Controller;

namespace Game
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private GameObject _decalPrefab;
        [SerializeField] private GameObject _projectilePrefab;

        private ITrajectoryStrategy _trajectoryStrategy;

        private void Awake()
        {
            ServiceLocator.RegisterMeshGeneratorService(new MeshGeneratorService());
            ServiceLocator.RegisterExplosionEffectService(new ExplosionFactory(_explosionPrefab));
            ServiceLocator.RegisterTrajectoryStrategy(_trajectoryStrategy);

            var cannonController = GameObject.FindAnyObjectByType<CannonController>();
            cannonController.Initialize(_projectilePrefab);

            var visualizer = GameObject.FindAnyObjectByType<TrajectoryVisualizer>();
            ServiceLocator.RegisterTrajectoryVisualizer(visualizer);

            var uiManager = GameObject.FindAnyObjectByType<UIManager>();
            ServiceLocator.RegisterUIManager(uiManager);

            var decalManagers = Object.FindObjectsByType<DecalManager>(FindObjectsSortMode.None);
            for (var i = 0; i < decalManagers.Length; i++)
            {
                decalManagers[i].Initialize(_decalPrefab);
            }

            ServiceLocator.RegisterUIManager(uiManager);
        }
    }
}
