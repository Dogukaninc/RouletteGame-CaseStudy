using System;
using _RouletteGame.Utilities;
using UnityEngine;

namespace _Space_Shooter_Files.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sources;
        private void Awake()
        {
            foreach (Sound s in sources)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }
        private void Start()
        {
            PlaySound(Tags.SOUND_TAG_THEME);
        }
    
        public void PlaySound(string name)
        {
            Sound source = Array.Find(sources, source => source.name == name);
            if (source == null) return;
            source.source.Play();
            Debug.Log("Sound Played: " + source.name);
      
        }
    }
}