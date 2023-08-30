using Asteroids.ECS;
using Asteroids.Model.Services;
using Asteroids.Model.SharedData;

namespace Asteroids.Model
{
    public class SpaceshipCollisionSystem : IUpdateSystem
    {
        private readonly IUiService _uiService;

        public SpaceshipCollisionSystem(IUiService uiService)
        {
            _uiService = uiService;
        }
        
        public void OnStart(World world)
        {
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            ref var gameStateData = ref world.GetSharedData<GameStateData>();

            var spaceshipEntity = entityManager.GetSingleEntity<SpaceshipComponent>();
            if (spaceshipEntity < 0)
            {
                return;
            }
            
            foreach (var entity in entityManager.Query<PhysicsCollisionComponent>())
            {
                ref var collisionComponent = ref entityManager.GetComponent<PhysicsCollisionComponent>(entity);
                if (collisionComponent.EntityA == spaceshipEntity ||
                    collisionComponent.EntityB == spaceshipEntity)
                {
                    gameStateData.IsGameOver = true;
                    entityManager.DestroyEntity(spaceshipEntity);
                    _uiService.ShowGameOverWindow(gameStateData.Score);
                    break;
                }
            }
        }

        public void OnStop(World world)
        {
            
        }
    }
}