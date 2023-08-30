using Asteroids.ECS;
using Asteroids.Model.SharedData;
using Asteroids.Model.Utility;

namespace Asteroids.Model
{
    public class AsteroidDestroySystem : IUpdateSystem
    {
        private GameConfigData _gameConfig;
        
        public void OnStart(World world)
        {
            _gameConfig = world.GetSharedData<GameConfigData>();
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;

            ref var gameStateData = ref world.GetSharedData<GameStateData>();

            foreach (var collisionEntity in entityManager.Query<PhysicsCollisionComponent>())
            {
                ref var collisionComponent = ref entityManager.GetComponent<PhysicsCollisionComponent>(collisionEntity);

                var collidedWithWeaponProjectile =
                    entityManager.HasComponent<AsteroidComponent>(collisionComponent.EntityA) &&
                    (entityManager.HasComponent<BulletComponent>(collisionComponent.EntityB) ||
                     entityManager.HasComponent<LaserComponent>(collisionComponent.EntityB));
                
                if (collidedWithWeaponProjectile)
                {
                    ref var asteroidComponent = ref entityManager.GetComponent<AsteroidComponent>(collisionComponent.EntityA);
                    if (asteroidComponent.CanBreakIntoSmallParts)
                    {
                        var transformComponent = entityManager.GetComponent<TransformComponent>(collisionComponent.EntityA);
                        var physicsBodyComponent = entityManager.GetComponent<PhysicsBodyComponent>(collisionComponent.EntityA);
                        
                        CreateAsteroidPart(entityManager, transformComponent, physicsBodyComponent, -90f);
                        CreateAsteroidPart(entityManager, transformComponent, physicsBodyComponent, 90f);
                    }
                    
                    entityManager.DestroyEntity(collisionComponent.EntityA);
                    gameStateData.Score += _gameConfig.Asteroid.ScoreReward;
                }
            }
        }

        private void CreateAsteroidPart(EntityManager entityManager,
            TransformComponent parentTransform, PhysicsBodyComponent parentBody, float rotationOffset)
        {
            var entity = entityManager.CreateEntity();

            parentTransform.Angle += rotationOffset;
            parentBody.Velocity = parentBody.Velocity.RotateVector(rotationOffset) * 2f;
            var asteroidComponent = new AsteroidComponent(false);
            
            entityManager.SetComponent(entity, ref parentTransform);
            entityManager.SetComponent(entity, ref parentBody);
            entityManager.SetComponent(entity, ref asteroidComponent);
        }

        public void OnStop(World world)
        {
        }
    }
}