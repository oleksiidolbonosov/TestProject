using Game.Model;

namespace Game.Strategy
{
    public interface ITrajectoryStrategy
    {
        void CalculateTrajectory(ProjectileModel projectile, float deltaTime);
    }
}
