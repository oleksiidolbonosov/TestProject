using Game.Controller;
using Game.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public class ExplosionFactory : IExplosionEffectService
    {
        private ObjectPool<ExplosionController> _explosionPool;

        public ExplosionFactory(GameObject explosionPrefab, int initialPoolSize = 10)
        {
            var explosionController = explosionPrefab.GetComponent<ExplosionController>();
            _explosionPool = new ObjectPool<ExplosionController>(explosionController, initialPoolSize);
            ProjectileController.OnExplosion += SpawnExplosion;
        }

        private void SpawnExplosion(Vector3 position, Vector3 normal, GameObject hitObject)
        {
            var explosion = _explosionPool.Get();
            explosion.transform.position = position;
            explosion.transform.rotation = Quaternion.identity;

            ReturnExplosionEffectAfterDelay(explosion, 5000);
        }

        private async void ReturnExplosionEffectAfterDelay(ExplosionController explosion, int delay)
        {
            await Task.Delay(delay);
            _explosionPool.Return(explosion);
        }

        ~ExplosionFactory()
        {
            ProjectileController.OnExplosion -= SpawnExplosion;
        }

        public void ReturnExplosion(ExplosionController explosion)
        {
            _explosionPool.Return(explosion);
        }

        public void ClearPool()
        {
            _explosionPool.Clear();
        }
    }
}
