using Asteroids.ECS;
using Asteroids.Model;

namespace Asteroids.Frontend
{
    public class PlayerInputSystem : IUpdateSystem
    {
        private PlayerInput _playerInput;

        public void OnStart(World world)
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
        }

        public void OnUpdate(World world, float deltaTime)
        {
            ref var gameStateData = ref world.GetSharedData<GameStateData>();
            if (gameStateData.IsGameOver)
            {
                return;
            }
            
            var playerShipInput = _playerInput.Ship;
            var accelerateInput = playerShipInput.Accelerate.IsPressed();
            var rotateInput = playerShipInput.Rotate.ReadValue<float>();
            var laserShootInput = playerShipInput.LaserShoot.WasPressedThisFrame();
            var bulletShootInput = playerShipInput.BulletShoot.WasPressedThisFrame();

            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager
                         .Query<SpaceshipInputComponent>())
            {
                ref var spaceshipInputComponent = ref entityManager.GetComponent<SpaceshipInputComponent>(entity);
                spaceshipInputComponent.Accelerate = accelerateInput;
                spaceshipInputComponent.Rotate = rotateInput;
                spaceshipInputComponent.LaserShoot = laserShootInput;
                spaceshipInputComponent.BulletShoot = bulletShootInput;
            }
        }

        public void OnStop(World world)
        {
            _playerInput.Disable();
        }
    }
}