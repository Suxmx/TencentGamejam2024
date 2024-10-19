using System;
using UnityEngine;

namespace Framework.Audio
{
    /// <summary>
    /// Background music properties for the AudioManager
    /// </summary>
    [Serializable]
    public struct BackgroundMusic
    {
        /// <summary>
        /// The current clip of the background music.
        /// </summary>
        public AudioClip CurrentClip;

        /// <summary>
        /// The next clip that is about to be played.
        /// </summary>
        public AudioClip NextClip;

        /// <summary>
        /// The music transition.
        /// </summary>
        public MusicTransition MusicTransition;

        /// <summary>
        /// The duration of the transition.
        /// </summary>
        public float TransitionDuration;
    }

}