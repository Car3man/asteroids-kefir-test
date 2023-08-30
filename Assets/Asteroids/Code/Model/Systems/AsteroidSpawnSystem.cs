using System;
using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model.SharedData;
using Asteroids.Model.Utility;

namespace Asteroids.Model
{
    public class AsteroidSpawnSystem : IUpdateSystem
    {
        private readonly Random _rng = new();

        private AsteroidConfigData _asteroidConfig;
        private float _worldWidth;
        private float _worldHeight;
        private float _time;
        private float _lastSpawnTime = float.MinValue;
        
        public void OnStart(World world)
        {
            var gameConfig = world.GetSharedData<GameConfigData>();
            _asteroidConfig = gameConfig.Asteroid;
            _worldWidth = gameConfig.WorldWidth;
            _worldHeight = gameConfig.WorldHeight;
            _lastSpawnTime = 0f;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            if (_time - _lastSpawnTime >= _asteroidConfig.SpawnEverySeconds)
            {
                var entityManager = world.EntityManager;

                var spaceshipEntity = entityManager.GetSingleEntity<SpaceshipComponent>();
                if (spaceshipEntity < 0)
                {
                    return;
                }

                ref var spaceshipTransformComponent =
                    ref world.EntityManager.GetComponent<TransformComponent>(spaceshipEntity);
                var spaceshipPosition = spaceshipTransformComponent.Position;
                
                var entity = entityManager.CreateEntity();

                var asteroidPosition = GetNextAsteroidPosition();
                var asteroidRotation = GetNextAsteroidRotation();
                var asteroidScale = GetNextAsteroidScale();
                
                var transformComponent = new TransformComponent(asteroidPosition, asteroidRotation, asteroidScale);
                var physicsBodyComponent = new PhysicsBodyComponent
                {
                    Drag = _asteroidConfig.Drag,
                    CollisionLayer = _asteroidConfig.CollisionLayer,
                    CollisionMask = _asteroidConfig.CollisionMask,
                    Velocity = GetNextAsteroidVelocity(asteroidPosition, spaceshipPosition)
                };
                var asteroidComponent = new AsteroidComponent(true);
            
                entityManager.SetComponent(entity, ref transformComponent);
                entityManager.SetComponent(entity, ref physicsBodyComponent);
                entityManager.SetComponent(entity, ref asteroidComponent);
                
                _lastSpawnTime = _time;
            }
            
            _time += deltaTime;
        }

        private Vector2 GetNextAsteroidPosition()
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

        private float GetNextAsteroidRotation()
        {
            return _rng.Next(0, 360);
        }
        
        private float GetNextAsteroidScale()
        {
            return (float)_rng.NextDouble() * (_asteroidConfig.MaxScale - _asteroidConfig.MinScale) + _asteroidConfig.MinScale;
        }

        private Vector2 GetNextAsteroidVelocity(Vector2 asteroidPosition, Vector2 spaceshipPosition)
        {
            const float maxAngleOffset = 60f;
            var direction = spaceshipPosition - asteroidPosition;
            var angleOffset = (float)_rng.NextDouble() * maxAngleOffset - maxAngleOffset / 2f;
            var initialVelocity = (float)_rng.NextDouble() * (_asteroidConfig.MaxInitialVelocity - _asteroidConfig.MinInitialVelocity) +
                                  _asteroidConfig.MinInitialVelocity;
            return direction.RotateVector(angleOffset) * initialVelocity;
        }

        public void OnStop(World world)
        {
        }
    }
}