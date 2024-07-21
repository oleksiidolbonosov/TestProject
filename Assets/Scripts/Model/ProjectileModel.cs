using UnityEngine;

namespace Game.Model
{
    public class ProjectileModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Gravity { get; set; } = -9.81f;
        public float BounceFactor { get; set; } = 0.5f;
        public float Friction { get; set; } = 0.1f;
    }
}