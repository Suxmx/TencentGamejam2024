using System;
using Framework.Audio;
using UnityEngine;

namespace Framework
{
    public interface IAudioManager : IService
    {
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }
        public bool IsMusicOn { get; set; }
        public bool IsSoundOn { get; set; }
        public bool IsMasterMute { get; set; }
        public void PlayBGM(string clipName, MusicTransition transition, float transition_duration, float volume,
            float pitch, float playback_position = 0);

        public void PlayBGM(string clipName, MusicTransition transition = MusicTransition.Swift,
            float transition_duration = 1f);

        public void StopBGM();
        public void PauseBGM();
        public void ResumeBGM();

        public AudioSource PlaySFX(string clipName, float duration, float volume=1f,
            bool singleton = false, float pitch = 1f, Action callback = null);

        public AudioSource PlayOneShot(string clipName, float volume, float pitch = 1f,
            Action callback = null);

        public void PauseAllSFX();
        public void ResumeAllSFX();
        public void StopAllSFX();
    }
}