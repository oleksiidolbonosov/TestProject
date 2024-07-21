using Game.Model;
using UnityEngine;

namespace Game.Services
{
    public interface ITrajectoryVisualizer
    {
        void Initialize(ProjectileModel projectileModel, Transform firePoint);
        void DrawTrajectory();
    }
}
