using Asteroids.ECS;
using Asteroids.Model.Utility;
using Vector2 = System.Numerics.Vector2;

namespace Asteroids.Model
{
    public class PhysicsSystem : IFixedUpdateSystem
    {
        public void OnStart(World world)
        {
        }

        public void OnFixedUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;

            ResolveLinearVelocity(entityManager, deltaTime);
            CleanUpCollisions(entityManager);
            ResolveBodyCollisions(entityManager);
            ResolveRaycastCollisions(entityManager);
        }

        private static void ResolveLinearVelocity(EntityManager entityManager, float deltaTime)
        {
            foreach (var entity in entityManager.Query<TransformComponent, PhysicsBodyComponent>())
            {
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                ref var physicsBodyComponent = ref entityManager.GetComponent<PhysicsBodyComponent>(entity);

                physicsBodyComponent.Velocity += physicsBodyComponent.Acceleration * deltaTime;
                transformComponent.Position += physicsBodyComponent.Velocity * deltaTime;
                physicsBodyComponent.Velocity -= physicsBodyComponent.Velocity * physicsBodyComponent.Drag * deltaTime;
            }
        }

        private void CleanUpCollisions(EntityManager entityManager)
        {
            foreach (var entity in entityManager.Query<PhysicsCollisionComponent>())
            {
                entityManager.DestroyEntity(entity);
            }
        }

        private void ResolveBodyCollisions(EntityManager entityManager)
        {
            foreach (var entityA in entityManager.Query<TransformComponent, PhysicsBodyComponent>())
            {
                ref var entityTransformA = ref entityManager.GetComponent<TransformComponent>(entityA);
                ref var entityBodyA = ref entityManager.GetComponent<PhysicsBodyComponent>(entityA);
                var entityPositionA = entityTransformA.Position;
                var entityRadiusA = entityTransformA.Scale / 2f;
                
                foreach (var entityB in entityManager.Query<TransformComponent, PhysicsBodyComponent>())
                {
                    ref var entityTransformB = ref entityManager.GetComponent<TransformComponent>(entityB);
                    ref var entityBodyB = ref entityManager.GetComponent<PhysicsBodyComponent>(entityB);
                    var entityPositionB = entityTransformB.Position;
                    var entityRadiusB = entityTransformB.Scale / 2f;
                    
                    if (entityA == entityB)
                    {
                        continue;
                    }

                    if ((entityBodyA.CollisionMask & entityBodyB.CollisionLayer) == 0)
                    {
                        continue;
                    }
                    
                    if (MathUtility.IsCircleIntersectCircle(entityPositionA, entityRadiusA,
                            entityPositionB, entityRadiusB))
                    {
                        var collisionEntity = entityManager.CreateEntity();
                        var collisionComponent = new PhysicsCollisionComponent(entityA, entityB);
                        entityManager.SetComponent(collisionEntity, ref collisionComponent);
                    }
                }
            }
        }

        private void ResolveRaycastCollisions(EntityManager entityManager)
        {
            var upVector = new Vector2(0f, 1f);
            
            foreach (var raycastEntity in entityManager.Query<PhysicsRaycastComponent>())
            {
                ref var raycastTransform = ref entityManager.GetComponent<TransformComponent>(raycastEntity);
                ref var raycastComponent = ref entityManager.GetComponent<PhysicsRaycastComponent>(raycastEntity);

                var raycastStart = raycastTransform.Position;
                var raycastEnd = raycastStart + upVector.RotateVector(raycastTransform.Angle) * raycastComponent.Length;
                
                foreach (var bodyEntity in entityManager.Query<TransformComponent, PhysicsBodyComponent>())
                {
                    ref var bodyTransform = ref entityManager.GetComponent<TransformComponent>(bodyEntity);
                    ref var bodyComponent = ref entityManager.GetComponent<PhysicsBodyComponent>(bodyEntity);
                    
                    if ((raycastComponent.CollisionMask & bodyComponent.CollisionLayer) == 0)
                    {
                        continue;
                    }
                    
                    var bodyPosition = bodyTransform.Position;
                    var bodyRadius = bodyTransform.Scale / 2f;
                    
                    if (MathUtility.IsLineIntersectCircle(raycastStart, raycastEnd,
                            bodyPosition, bodyRadius))
                    {
                        var collisionEntity = entityManager.CreateEntity();
                        var collisionComponent = new PhysicsCollisionComponent(bodyEntity, raycastEntity);
                        entityManager.SetComponent(collisionEntity, ref collisionComponent);
                    }
                }
                
                entityManager.RemoveComponent<PhysicsRaycastComponent>(raycastEntity);
            }
        }

        public void OnStop(World world)
        {
        }
    }
}