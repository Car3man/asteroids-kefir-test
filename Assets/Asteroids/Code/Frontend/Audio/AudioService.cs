using Asteroids.Model;
using UnityEngine;

namespace Asteroids.Frontend.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource audioSource;
        
        public void PlaySound(string clipName)
        {
            var clip = ResourcesUtility.Load<AudioClip>($"Audio/{clipName}");
            audioSource.PlayOneShot(clip);
        }
    }
}