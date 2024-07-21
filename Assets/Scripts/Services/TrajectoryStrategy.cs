using Game.Model;
using Game.Strategy;
using UnityEngine;

namespace Game.Services
{
    public class TrajectoryStrategy : ITrajectoryStrategy
    {
        public void CalculateTrajectory(ProjectileModel projectile, float deltaTime)
        {
            projectile.Velocity += new Vector3(0, projectile.Gravity * deltaTime, 0);
            projectile.Position += projectile.Velocity * deltaTime;
        }
    }
  
}
