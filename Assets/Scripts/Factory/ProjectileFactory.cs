using Game.Controller;
using Game.Services;
using Game.Utils;
using UnityEngine;

namespace Game.Factory
{
    public class ProjectileFactory
    {
        private readonly ObjectPool<ProjectileController> _projectilePool;
        private readonly IMeshGeneratorService _meshGeneratorService;

        public ProjectileFactory(GameObject projectilePrefab, IMeshGeneratorService meshGeneratorService, int initialPoolSize = 10)
        {
            this._meshGeneratorService = meshGeneratorService;
            var projectileController = projectilePrefab.GetComponent<ProjectileController>();
            _projectilePool = new ObjectPool<ProjectileController>(projectileController, initialPoolSize);
        }

        public ProjectileController CreateProjectile(Vector3 position, Quaternion rotation)
        {
            var projectile = _projectilePool.Get();
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;

            var meshFilter = projectile.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = _meshGeneratorService.GenerateMesh(out var radius);
                projectile.Radius = radius;
            }

            return projectile;
        }

        public void ReturnProjectile(ProjectileController projectile)
        {
            _projectilePool.Return(projectile);
        }

        public void ClearPool()
        {
            _projectilePool.Clear();
        }
    }
}
