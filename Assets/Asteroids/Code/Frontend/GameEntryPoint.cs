using Asteroids.Frontend.Audio;
using UnityEngine;

namespace Asteroids.Frontend
{
    public class GameEntryPoint : MonoBehaviour
    {
        private AudioService _audioService;
        private UiService _uiService;
        private GameWorld _gameWorld;

        private void Awake()
        {
            ResolveDependencies();
            
            _gameWorld = new GameWorld(_audioService, _uiService);
            _uiService.SetGameWorld(_gameWorld);
        }

        private void Start()
        {
            _gameWorld.Start();
        }

        private void Update()
        {
            _gameWorld.Update();
        }

        private void FixedUpdate()
        {
            _gameWorld.FixedUpdate();
        }

        private void OnDestroy()
        {
            _gameWorld.Stop();
        }

        private void ResolveDependencies()
        {
            var audioService = FindObjectOfType<AudioService>();
            if (audioService == null)
            {
                throw new System.NullReferenceException(nameof(audioService));
            }
            
            var uiService = FindObjectOfType<UiService>();
            if (uiService == null)
            {
                throw new System.NullReferenceException(nameof(uiService));
            }

            _audioService = audioService;
            _uiService = uiService;
        }
    }
}