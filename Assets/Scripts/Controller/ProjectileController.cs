using Game.Model;
using UnityEngine;
using Game.Factory;

namespace Game.Controller
{
    public class ProjectileController : MonoBehaviour
    {
        public delegate void EventHandler(Vector3 position, Vector3 normal, GameObject hitObject);
        public static event EventHandler OnExplosion;
        public static event EventHandler OnAddDecal;

        private ProjectileFactory _projectileFactory;
        private ProjectileModel _projectileModel;
        private int _groundCollisionCount = 0;
        private int _wallCollisionCount = 0;
        private float _radius;

        public float Radius { get { return _radius; } set { _radius = value; } }

        public void Initialize(ProjectileFactory projectileFactory, Vector3 initialVelocity, Quaternion initialRotation)
        {
            _projectileFactory = projectileFactory;
            _projectileModel = new ProjectileModel
            {
                Position = transform.position,
                Velocity = initialVelocity,
            };

            _projectileModel.Velocity = initialVelocity;
            transform.rotation = initialRotation;
        }

        private void FixedUpdate()
        {
            _projectileModel.Position += _projectileModel.Velocity * Time.fixedDeltaTime;
            _projectileModel.Velocity += new Vector3(0, _projectileModel.Gravity, 0) * Time.fixedDeltaTime;

            transform.position = _projectileModel.Position;
            DetectCollisions();
        }

        private void DetectCollisions()
        {
            var rayDirection = _projectileModel.Velocity.normalized;
            var sphereCastRadius = _radius;

            if (Physics.SphereCast(transform.position, sphereCastRadius/2f, rayDirection, out var hit, _projectileModel.Velocity.magnitude * Time.deltaTime))
            {
                Collider hitCollider = hit.collider;
                Vector3 contactPoint = hit.point;

                if (hitCollider.CompareTag("Ground"))
                {
                    HandleGroundCollision(hit.normal, contactPoint, hitCollider.gameObject);
                }
                else if (hitCollider.CompareTag("Wall"))
                {
                    HandleWallCollision(hit.normal, contactPoint, hitCollider.gameObject);
                }
            }
        }

        private void HandleGroundCollision(Vector3 normal, Vector3 contactPoint, GameObject hitObject)
        {
            if (_groundCollisionCount == 0)
            {
                _projectileModel.Velocity = Vector3.Reflect(_projectileModel.Velocity, normal);

                _projectileModel.Velocity = new Vector3(
                    0,
                    _projectileModel.Velocity.y * _projectileModel.BounceFactor,
                    0
                );

                _groundCollisionCount++;
            }
            else if (_groundCollisionCount == 1)
            {
                HandleCollision(contactPoint, normal, hitObject);
            }
        }


        private void HandleWallCollision(Vector3 normal, Vector3 contactPoint, GameObject hitObject)
        {
            _wallCollisionCount++;

            if (_wallCollisionCount == 2)
            {
                HandleCollision(contactPoint, normal, hitObject);
            }
            else
            {
                _projectileModel.Velocity = Vector3.Reflect(_projectileModel.Velocity, normal);
                _projectileModel.Velocity *= (1 - _projectileModel.Friction*5);
            }

            CreateDecal(contactPoint, normal, hitObject);
        }

        private void HandleCollision(Vector3 contactPoint, Vector3 normal, GameObject hitObject)
        {
            CreateExplosionEffect(contactPoint, normal, hitObject);
            _projectileFactory.ReturnProjectile(this);
            _wallCollisionCount = 0;
            _groundCollisionCount = 0;
        }

        private void CreateDecal(Vector3 position, Vector3 normal, GameObject hitObject)
        {
            OnAddDecal?.Invoke(position, normal, hitObject);
        }

        private void CreateExplosionEffect(Vector3 position, Vector3 normal, GameObject hitObject)
        {
            OnExplosion?.Invoke(position, normal, hitObject);
        }
    }
}
