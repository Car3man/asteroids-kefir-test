using Asteroids.Model;
using TMPro;
using UnityEngine;

namespace Asteroids.Frontend
{
    public class UiService : MonoBehaviour, IUiService
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI spaceshipInfoText;
        [SerializeField] private GameOverWindow gameOverWindow;
        
        private GameWorld _gameWorld;

        public void SetGameWorld(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
        }
        
        private void Start()
        {
            gameOverWindow.gameObject.SetActive(false);
        }

        private void Update()
        {
            var ecsWorld = _gameWorld.EcsWorld;
            
            if (!ecsWorld.IsRunning)
            {
                return;
            }
            
            var entityManager = ecsWorld.EntityManager;

            if (!entityManager.HasComponent<SpaceshipComponent>())
            {
                return;   
            }

            ref var gameStateData = ref ecsWorld.GetSharedData<GameStateData>();
            var spaceshipEntity = entityManager.GetSingleEntity<SpaceshipComponent>();
            ref var spaceship = ref entityManager.GetComponent<SpaceshipComponent>(spaceshipEntity);
            ref var spaceshipTransform = ref entityManager.GetComponent<TransformComponent>(spaceshipEntity);
            ref var spaceshipBody = ref entityManager.GetComponent<PhysicsBodyComponent>(spaceshipEntity);

            scoreText.text = $"Score: {gameStateData.Score}";
            spaceshipInfoText.text = $"Position: {spaceshipTransform.Position}\n\r" +
                                     $"Angle: {spaceshipTransform.Angle}\n\r" +
                                     $"Velocity: {spaceshipBody.Velocity}\n\r" +
                                     $"Laser Charges: {spaceship.LaserCharges}\n\r" +
                                     $"Laser Cooldown: {spaceship.LaserCooldownTimeDown}";
        }

        public void ShowGameOverWindow(int score)
        {
            gameOverWindow.SetScore(score);
            gameOverWindow.RestartButtonClicked += RestartButtonClicked;
            gameOverWindow.gameObject.SetActive(true);

            void RestartButtonClicked()
            {
                gameOverWindow.RestartButtonClicked -= RestartButtonClicked;
                gameOverWindow.gameObject.SetActive(false);
                
                _gameWorld.Restart();
            }
        }
    }
}