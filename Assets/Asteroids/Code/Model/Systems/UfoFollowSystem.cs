using System.Numerics;
using Asteroids.ECS;

namespace Asteroids.Model
{
    public class UfoFollowSystem : IUpdateSystem
    {
        private UfoConfigData _ufoConfig;

        public void OnStart(World world)
        {
            _ufoConfig = world.GetSharedData<GameConfigData>().Ufo;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            var spaceshipEntity = entityManager.GetSingleEntity<SpaceshipComponent>();
            if (spaceshipEntity < 0)
            {
                return;
            }

            ref var spaceshipTransformComponent = ref entityManager.GetComponent<TransformComponent>(spaceshipEntity);
            var spaceshipPosition = spaceshipTransformComponent.Position;

            foreach (var entity in entityManager
                         .Query<UfoComponent, TransformComponent, PhysicsBodyComponent>())
            {
                ref var ufoComponent = ref entityManager.GetComponent<UfoComponent>(entity);
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                ref var physicsBodyComponent = ref entityManager.GetComponent<PhysicsBodyComponent>(entity);

                var ufoPosition = transformComponent.Position;
                var followVector = Vector2.Normalize(spaceshipPosition - ufoPosition) * _ufoConfig.MoveSpeed;
                physicsBodyComponent.Acceleration = followVector;
            }
        }

        public void OnStop(World world)
        {
        }
    }
}
