using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model.Services;
using Asteroids.Model.SharedData;
using Asteroids.Model.Utility;

namespace Asteroids.Model
{
    public class SpaceshipLaserShootSystem : IUpdateSystem
    {
        private readonly IAudioService _audioService;
        private SpaceshipConfigData _spaceshipConfigData;
        private LaserConfigData _laserConfigData;

        public SpaceshipLaserShootSystem(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        public void OnStart(World world)
        {
            var gameConfig = world.GetSharedData<GameConfigData>();
            _spaceshipConfigData = gameConfig.Spaceship;
            _laserConfigData = gameConfig.Laser;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            HandleLasersLifetime(entityManager, deltaTime);
            HandleLaserCharges(entityManager, deltaTime);
            HandleInput(entityManager);
        }

        private static void HandleLasersLifetime(EntityManager entityManager, float deltaTime)
        {
            foreach (var entity in entityManager.Query<LaserComponent>())
            {
                ref var laserComponent = ref entityManager.GetComponent<LaserComponent>(entity);

                laserComponent.LifetimeTimeDown -= deltaTime;
                if (laserComponent.LifetimeTimeDown <= 0f)
                {
                    entityManager.DestroyEntity(entity);
                }
            }
        }

        private void HandleLaserCharges(EntityManager entityManager, float deltaTime)
        {
            foreach (var entity in entityManager.Query<SpaceshipComponent>())
            {
                ref var spaceshipComponent = ref entityManager.GetComponent<SpaceshipComponent>(entity);
                
                if (spaceshipComponent.LaserCharges >= _spaceshipConfigData.LaserCapacity)
                {
                    spaceshipComponent.LaserCooldownTimeDown = _spaceshipConfigData.LaserCooldown;
                    continue;
                }

                if (spaceshipComponent.LaserCooldownTimeDown <= 0f)
                {
                    spaceshipComponent.LaserCharges++;
                    spaceshipComponent.LaserCooldownTimeDown = _spaceshipConfigData.LaserCooldown;
                }
                else
                {
                    spaceshipComponent.LaserCooldownTimeDown -= deltaTime;
                }
            }
        }

        private void HandleInput(EntityManager entityManager)
        {
            foreach (var entity in entityManager
                         .Query<SpaceshipComponent, SpaceshipInputComponent, TransformComponent>())
            {
                ref var spaceshipComponent = ref entityManager.GetComponent<SpaceshipComponent>(entity);
                if (spaceshipComponent.LaserCharges <= 0)
                {
                    continue;
                }
                
                ref var spaceshipInputComponent = ref entityManager.GetComponent<SpaceshipInputComponent>(entity);
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);

                if (spaceshipInputComponent.LaserShoot)
                {
                    spaceshipComponent.LaserCharges--;
                    CreateLaser(entityManager, transformComponent.Position, transformComponent.Angle, transformComponent.Scale);
                    _audioService.PlaySound("LaserShoot");
                }
            }
        }

        private void CreateLaser(EntityManager entityManager, Vector2 spaceshipPosition, float spaceshipAngle, float spaceshipScale)
        {
            var up = new Vector2(0f, 1f);
            var spaceshipDirection = up.RotateVector(spaceshipAngle);
            var spaceshipShootOrigin = spaceshipPosition + spaceshipDirection * (spaceshipScale / 2f);
            
            var entity = entityManager.CreateEntity();
            
            var transformComponent = new TransformComponent(spaceshipShootOrigin, spaceshipAngle);
            var physicsRaycastComponent = new PhysicsRaycastComponent(_laserConfigData.Length, _laserConfigData.CollisionMask);
            var laserComponent = new LaserComponent(_laserConfigData.Lifetime);
            
            entityManager.SetComponent(entity, ref transformComponent);
            entityManager.SetComponent(entity, ref physicsRaycastComponent);
            entityManager.SetComponent(entity, ref laserComponent);
        }

        public void OnStop(World world)
        {
        }
    }
}