using System;
using System.Numerics;
using Asteroids.ECS;

namespace Asteroids.Model
{
    public class UfoSpawnSystem : IUpdateSystem
    {
        private readonly Random _rng = new();

        private UfoConfigData _ufoConfig;
        private float _worldWidth;
        private float _worldHeight;
        private float _time;
        private float _lastSpawnTime = float.MinValue;

        public void OnStart(World world)
        {
            var gameConfig = world.GetSharedData<GameConfigData>();
            _ufoConfig = gameConfig.Ufo;
            _worldWidth = gameConfig.WorldWidth;
            _worldHeight = gameConfig.WorldHeight;
            _lastSpawnTime = 0f;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            if (_time - _lastSpawnTime >= _ufoConfig.SpawnEverySeconds)
            {
                var entityManager = world.EntityManager;

                var spaceshipEntity = entityManager.GetSingleEntity<SpaceshipComponent>();
                if (spaceshipEntity < 0)
                {
                    return;
                }
                
                var entity = entityManager.CreateEntity();
                
                var transformComponent = new TransformComponent(GetNextUfoPosition(), 0f, _ufoConfig.Scale);
                var physicsBodyComponent = new PhysicsBodyComponent(_ufoConfig.Drag, _ufoConfig.CollisionLayer, _ufoConfig.CollisionMask);
                var ufoComponent = new UfoComponent();
            
                entityManager.SetComponent(entity, ref transformComponent);
                entityManager.SetComponent(entity, ref physicsBodyComponent);
                entityManager.SetComponent(entity, ref ufoComponent);
                
                _lastSpawnTime = _time;
            }
            
            _time += deltaTime;
        }
        
        private Vector2 GetNextUfoPosition()
        {
            var position = new Vector2();
            
            var worldWidthExt = _worldWidth * 1.1f;
            var worldHeightExt = _worldHeight * 1.1f;
            
            var isTopBottom = _rng.Next(0, 2);
            if (isTopBottom == 1)
            {
                position.X = (float)_rng.NextDouble() * worldWidthExt - worldWidthExt / 2f;
                
                var isTop = _rng.Next(0, 2);
                if (isTop == 1)
                {
                    position.Y = worldHeightExt / 2f;
                }
                else
                {
                    position.Y = -worldHeightExt / 2f;
                }
            }
            else
            {
                position.Y = (float)_rng.NextDouble() * worldHeightExt - worldHeightExt / 2f;
                
                var isRight = _rng.Next(0, 2);
                if (isRight == 1)
                {
                    position.X = worldWidthExt / 2f;
                }
                else
                {
                    position.X = -worldWidthExt / 2f;
                }
            }

            return position;
        }

        public void OnStop(World world)
        {
        }
    }
}