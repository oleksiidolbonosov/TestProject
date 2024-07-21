using Game.Controller;
namespace Game.Services
{
    public interface IExplosionEffectService
    {
        void ReturnExplosion(ExplosionController explosion);

        void ClearPool();
    }
}
