using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model.SharedData;

namespace Asteroids.Model
{
    public class SpaceshipTeleportSystem : IUpdateSystem
    {
        private Vector2 _minWorldBounds;
        private Vector2 _maxWorldBounds;
        
        public void OnStart(World world)
        {
            var gameConfig = world.GetSharedData<GameConfigData>();
            var worldWidth = gameConfig.WorldWidth;
            var worldHeight = gameConfig.WorldHeight;
            _minWorldBounds = new Vector2(-worldWidth / 2f, -worldHeight / 2f);
            _maxWorldBounds = new Vector2(worldWidth / 2f, worldHeight / 2f);
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager.Query<SpaceshipComponent, TransformComponent>())
            {
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                var position = transformComponent.Position;
                var halfScale = transformComponent.Scale / 2f;

                if (position.X < _minWorldBounds.X - halfScale)
                {
                    position.X = _maxWorldBounds.X + halfScale * 0.99f;
                }
                
                if (position.X > _maxWorldBounds.X + halfScale)
                {
                    position.X = _minWorldBounds.X - halfScale * 0.99f;
                }

                if (position.Y < _minWorldBounds.Y - halfScale)
                {
                    position.Y = _maxWorldBounds.Y + halfScale * 0.99f;
                }
                
                if (position.Y > _maxWorldBounds.Y + halfScale)
                {
                    position.Y = _minWorldBounds.Y - halfScale * 0.99f;
                }

                transformComponent.Position = position;
            }
        }

        public void OnStop(World world)
        {
        }
    }
}