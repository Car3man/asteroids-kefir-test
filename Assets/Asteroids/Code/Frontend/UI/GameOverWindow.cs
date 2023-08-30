using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Frontend
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button restartButton;
        
        public delegate void RestartButtonClick();
        public event RestartButtonClick RestartButtonClicked;
        
        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnButtonRestartClick);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnButtonRestartClick);
        }

        public void SetScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void OnButtonRestartClick()
        {
            RestartButtonClicked?.Invoke();
        }
    }
}