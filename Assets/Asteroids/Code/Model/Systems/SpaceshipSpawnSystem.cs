using Asteroids.ECS;
using Asteroids.Model.SharedData;
using Vector2 = System.Numerics.Vector2;

namespace Asteroids.Model
{
    public class SpaceshipSpawnSystem : ISystem
    {
        public void OnStart(World world)
        {
            var spaceshipConfig = world.GetSharedData<GameConfigData>().Spaceship;
            
            var entityManager = world.EntityManager;
            var entity = entityManager.CreateEntity();
            
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, spaceshipConfig.Scale);
            var physicsBodyComponent = new PhysicsBodyComponent(spaceshipConfig.Drag, spaceshipConfig.CollisionLayer, spaceshipConfig.CollisionMask);
            var spaceshipComponent = new SpaceshipComponent(spaceshipConfig.LaserCapacity);
            var spaceshipInputComponent = new SpaceshipInputComponent();
            
            entityManager.SetComponent(entity, ref transformComponent);
            entityManager.SetComponent(entity, ref physicsBodyComponent);
            entityManager.SetComponent(entity, ref spaceshipComponent);
            entityManager.SetComponent(entity, ref spaceshipInputComponent);
        }

        public void OnStop(World world)
        {
        }
    }
}