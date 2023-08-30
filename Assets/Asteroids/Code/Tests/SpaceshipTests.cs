using System;
using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model;
using Asteroids.Tests.DummyServices;
using NUnit.Framework;

namespace Asteroids.Tests
{
    public class SpaceshipTests
    {
        private World _world;

        [SetUp]
        public void SetUp()
        {
            _world = new World();
            _world.RegisterComponent<TransformComponent>();
            _world.RegisterComponent<PhysicsBodyComponent>();
            _world.RegisterComponent<SpaceshipComponent>();
            _world.RegisterComponent<SpaceshipInputComponent>();
            _world.RegisterComponent<BulletComponent>();
            _world.RegisterSharedData<GameConfigData>();
            _world.RegisterSystem(new SpaceshipMoveSystem());
            _world.RegisterSystem(new SpaceshipBulletShootSystem(new DummyAudioService()));

            var gameConfig = new GameConfigData
            {
                Spaceship = new SpaceshipConfigData { MoveSpeed = 10f, RotateSpeed = 10f }
            };
            _world.SetSharedData(ref gameConfig);
            _world.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _world.Stop(true);
            _world = null;
        }

        [Test]
        public void When_NoAccelerateInput_Then_SpaceshipShouldHasZeroAcceleration()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            var spaceshipComponent = new SpaceshipComponent();
            var spaceshipInputComponent = new SpaceshipInputComponent
            {
                Accelerate = false
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipInputComponent);
            
            // Act
            _world.Update(1f);
            
            // Assert
            ref var assertBodyComponent = ref _world.EntityManager.GetComponent<PhysicsBodyComponent>(entity);
            Assert.AreEqual(0f, assertBodyComponent.Acceleration.Length(), Math.E,
                "Spaceship hasn't accelerate input, but changed acceleration.");
        }

        [Test]
        public void When_AccelerateInput_Then_SpaceshipShouldHasPositiveAcceleration()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            var spaceshipComponent = new SpaceshipComponent();
            var spaceshipInputComponent = new SpaceshipInputComponent
            {
                Accelerate = true
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipInputComponent);
            
            // Act
            _world.Update(1f);
            
            // Assert
            ref var assertBodyComponent = ref _world.EntityManager.GetComponent<PhysicsBodyComponent>(entity);
            Assert.Greater(assertBodyComponent.Acceleration.Length(), 0f,
                "Spaceship has accelerate input, but didn't changed acceleration.");
        }
        
        [Test]
        public void When_NoRotateInput_Then_SpaceshipShouldHasSameAngle()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            var spaceshipComponent = new SpaceshipComponent();
            var spaceshipInputComponent = new SpaceshipInputComponent
            {
                Rotate = 0f
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipInputComponent);
            
            // Act
            _world.Update(1f);
            
            // Assert
            ref var assertTransformComponent = ref _world.EntityManager.GetComponent<TransformComponent>(entity);
            Assert.AreEqual(0f, assertTransformComponent.Angle, Math.E,
                "Spaceship hasn't rotate input, but changed angle.");
        }
        
        [Test]
        public void When_RotateInput_Then_SpaceshipShouldChangeAngle()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            var spaceshipComponent = new SpaceshipComponent();
            var spaceshipInputComponent = new SpaceshipInputComponent
            {
                Rotate = 1f
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipInputComponent);
            
            // Act
            _world.Update(1f);
            
            // Assert
            ref var assertTransformComponent = ref _world.EntityManager.GetComponent<TransformComponent>(entity);
            Assert.Greater(assertTransformComponent.Angle, 0f,
                "Spaceship has rotate input, but didn't changed angle.");
        }
        
        [Test]
        public void When_BulletShootInput_Then_SpaceshipShouldCreateBullet()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            var spaceshipComponent = new SpaceshipComponent();
            var spaceshipInputComponent = new SpaceshipInputComponent
            {
                BulletShoot = true
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipComponent);
            _world.EntityManager.SetComponent(entity, ref spaceshipInputComponent);
            
            // Act
            _world.Update(1f);
            
            // Assert
            var isThereBulletComponentInWorld = _world.EntityManager.HasComponent<BulletComponent>();
            Assert.IsTrue(isThereBulletComponentInWorld,
                "Spaceship has bullet shoot input, but didn't created a bullet.");
        }
    }
}