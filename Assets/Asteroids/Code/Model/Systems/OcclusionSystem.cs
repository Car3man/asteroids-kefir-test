using Asteroids.ECS;

namespace Asteroids.Model
{
    public class OcclusionSystem : IUpdateSystem
    {
        private float _occlusionDistance;
        
        public void OnStart(World world)
        {
            _occlusionDistance = world.GetSharedData<GameConfigData>().OcclusionDistance;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager.Query<TransformComponent>())
            {
                if (entityManager.HasComponent<AsteroidComponent>() ||
                    entityManager.HasComponent<UfoComponent>() ||
                    entityManager.HasComponent<BulletComponent>())
                {
                    ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                    var distance = transformComponent.Position.Length();
                    if (distance > _occlusionDistance)
                    {
                        entityManager.DestroyEntity(entity);
                    }
                }
            }
        }

        public void OnStop(World world)
        {
        }
    }
}