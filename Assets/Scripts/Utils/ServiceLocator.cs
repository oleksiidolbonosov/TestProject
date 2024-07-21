using Game.Services;
using Game.Strategy;

namespace Game
{
    public static class ServiceLocator
    {
        private static IMeshGeneratorService _meshGeneratorService;
        private static ITrajectoryStrategy _trajectoryStrategy;
        private static IExplosionEffectService _explosionEffectService;
        private static ITrajectoryVisualizer _trajectoryVisualizer;
        private static IUIManager _uiManager;

        public static void RegisterMeshGeneratorService(IMeshGeneratorService service)
        {
            _meshGeneratorService = service;
        }
        
        public static void RegisterTrajectoryStrategy(ITrajectoryStrategy strategy)
        {
            _trajectoryStrategy = strategy;
        }

        public static void RegisterExplosionEffectService(IExplosionEffectService service)
        {
            _explosionEffectService = service;
        }

        public static void RegisterTrajectoryVisualizer(ITrajectoryVisualizer visualizer)
        {
            _trajectoryVisualizer = visualizer;
        }

        public static void RegisterUIManager(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public static IMeshGeneratorService GetMeshGeneratorService()
        {
            return _meshGeneratorService;
        }

        public static ITrajectoryStrategy GetTrajectoryStrategy()
        {
            return _trajectoryStrategy;
        }

        public static IExplosionEffectService GetExplosionEffectService()
        {
            return _explosionEffectService;
        }

        public static ITrajectoryVisualizer GetTrajectoryVisualizer()
        {
            return _trajectoryVisualizer;
        } 
        
        public static IUIManager GetUIManager()
        {
            return _uiManager;
        }
    }
}
