using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model.Services;
using Asteroids.Model.SharedData;
using Asteroids.Model.Utility;

namespace Asteroids.Model
{
    public class SpaceshipBulletShootSystem : IUpdateSystem
    {
        private readonly IAudioService _audioService;
        private BulletConfigData _bulletConfig;

        public SpaceshipBulletShootSystem(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        public void OnStart(World world)
        {
            _bulletConfig = world.GetSharedData<GameConfigData>().Bullet;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager
                         .Query<SpaceshipInputComponent, TransformComponent>())
            {
                ref var spaceshipInputComponent = ref entityManager.GetComponent<SpaceshipInputComponent>(entity);
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);

                if (spaceshipInputComponent.BulletShoot)
                {
                    CreateBullet(entityManager, transformComponent.Position, transformComponent.Angle, transformComponent.Scale);
                    _audioService.PlaySound("BulletShoot");
                }
            }
        }

        private void CreateBullet(EntityManager entityManager, Vector2 spaceshipPosition, float spaceshipAngle, float spaceshipScale)
        {
            var up = new Vector2(0f, 1f);
            var spaceshipDirection = up.RotateVector(spaceshipAngle);
            var spaceshipShootOrigin = spaceshipPosition + spaceshipDirection * (spaceshipScale / 2f);
            var initialVelocity = spaceshipDirection * _bulletConfig.InitialVelocity;
            
            var entity = entityManager.CreateEntity();
            
            var transformComponent = new TransformComponent(spaceshipShootOrigin, spaceshipAngle, _bulletConfig.Scale);
            var physicsBodyComponent = new PhysicsBodyComponent(_bulletConfig.Drag, _bulletConfig.CollisionLayer, _bulletConfig.CollisionMask)
            {
                Velocity = initialVelocity
            };
            var bulletComponent = new BulletComponent();
            
            entityManager.SetComponent(entity, ref transformComponent);
            entityManager.SetComponent(entity, ref physicsBodyComponent);
            entityManager.SetComponent(entity, ref bulletComponent);
        }

        public void OnStop(World world)
        {
        }
    }
}