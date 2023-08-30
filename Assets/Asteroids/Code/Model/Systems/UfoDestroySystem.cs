using Asteroids.ECS;
using Asteroids.Model.SharedData;

namespace Asteroids.Model
{
    public class UfoDestroySystem : IUpdateSystem
    {
        private UfoConfigData _ufoConfig;
        
        public void OnStart(World world)
        {
            _ufoConfig = world.GetSharedData<GameConfigData>().Ufo;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;

            ref var gameStateData = ref world.GetSharedData<GameStateData>();
            
            foreach (var collisionEntity in entityManager.Query<PhysicsCollisionComponent>())
            {
                ref var collisionComponent = ref entityManager.GetComponent<PhysicsCollisionComponent>(collisionEntity);

                var collidedWithWeaponProjectile =
                    entityManager.HasComponent<UfoComponent>(collisionComponent.EntityA) &&
                    (entityManager.HasComponent<BulletComponent>(collisionComponent.EntityB) ||
                     entityManager.HasComponent<LaserComponent>(collisionComponent.EntityB));

                if (collidedWithWeaponProjectile)
                {
                    entityManager.DestroyEntity(collisionComponent.EntityA);
                    gameStateData.Score += _ufoConfig.ScoreReward;
                }
            }
        }

        public void OnStop(World world)
        {
        }
    }
}