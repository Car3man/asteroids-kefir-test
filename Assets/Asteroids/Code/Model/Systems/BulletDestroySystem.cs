using Asteroids.ECS;

namespace Asteroids.Model
{
    public class BulletDestroySystem : IUpdateSystem
    {
        public void OnStart(World world)
        {
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var collisionEntity in entityManager.Query<PhysicsCollisionComponent>())
            {
                ref var collisionComponent = ref entityManager.GetComponent<PhysicsCollisionComponent>(collisionEntity);

                var shouldBeDestroyed =
                    entityManager.HasComponent<BulletComponent>(collisionComponent.EntityA);
                if (shouldBeDestroyed)
                {
                    entityManager.DestroyEntity(collisionComponent.EntityA);
                }
            }
        }

        public void OnStop(World world)
        {
        }
    }
}