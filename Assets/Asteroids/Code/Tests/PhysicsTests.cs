using System;
using System.Numerics;
using Asteroids.ECS;
using Asteroids.Model;
using NUnit.Framework;

namespace Asteroids.Tests
{
    public class PhysicsTests
    {
        private World _world;

        [SetUp]
        public void SetUp()
        {
            _world = new World();
            _world.RegisterComponent<TransformComponent>();
            _world.RegisterComponent<PhysicsBodyComponent>();
            _world.RegisterComponent<PhysicsCollisionComponent>();
            _world.RegisterComponent<PhysicsRaycastComponent>();
            _world.RegisterSystem(new PhysicsSystem());
            _world.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _world.Stop(true);
            _world = null;
        }
        
        [Test]
        public void When_VelocityEqualsZero_Then_BodyShouldNotMove()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = Vector2.Zero
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            
            // Act
            _world.FixedUpdate(1f);
            
            // Assert
            ref var assertTransformComponent = ref _world.EntityManager.GetComponent<TransformComponent>(entity);
            Assert.AreEqual(0f, assertTransformComponent.Position.Length(), Math.E,
                "Physics body moved with zero velocity.");
        }
        
        [Test]
        public void When_VelocityEqualsNotZero_Then_BodyShouldMove()
        {
            // Arrange
            var entity = _world.EntityManager.CreateEntity();
            var transformComponent = new TransformComponent(Vector2.Zero, 0f, 1f);
            var bodyComponent = new PhysicsBodyComponent(1f, 0, 0)
            {
                Acceleration = Vector2.Zero,
                Velocity = new Vector2(0f, 1f)
            };
            _world.EntityManager.SetComponent(entity, ref transformComponent);
            _world.EntityManager.SetComponent(entity, ref bodyComponent);
            
            // Act
            _world.FixedUpdate(1f);
            
            // Assert
            ref var assertTransformComponent = ref _world.EntityManager.GetComponent<TransformComponent>(entity);
            Assert.AreNotEqual(0f, assertTransformComponent.Position.Length(),
                "Physics body didn't moved with not zero velocity.");
        }
    }
}
