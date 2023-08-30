using System;
using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model.SharedData;
using Asteroids.Model.Utility;

namespace Asteroids.Model
{
    public class SpaceshipMoveSystem : IUpdateSystem
    {
        private SpaceshipConfigData _spaceshipConfig;

        public void OnStart(World world)
        {
            _spaceshipConfig = world.GetSharedData<GameConfigData>().Spaceship;
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager
                         .Query<SpaceshipInputComponent, TransformComponent, PhysicsBodyComponent>())
            {
                ref var spaceshipInputComponent = ref entityManager.GetComponent<SpaceshipInputComponent>(entity);
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                ref var physicsBodyComponent = ref entityManager.GetComponent<PhysicsBodyComponent>(entity);

                transformComponent.Angle -= spaceshipInputComponent.Rotate * _spaceshipConfig.RotateSpeed * deltaTime;
                if (transformComponent.Angle < 0f - Math.E)
                {
                    transformComponent.Angle = 360f;
                }
                if (transformComponent.Angle > 360f + Math.E)
                {
                    transformComponent.Angle = 0f;
                }
                
                var accelerateVector = Vector2.Zero;
                if (spaceshipInputComponent.Accelerate)
                {
                    var up = new Vector2(0f, 1f);
                    accelerateVector = up.RotateVector(transformComponent.Angle);
                }
                physicsBodyComponent.Acceleration = accelerateVector * _spaceshipConfig.MoveSpeed;
            }
        }

        public void OnStop(World world)
        {
        }
    }
}