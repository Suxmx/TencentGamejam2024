using System;
using UnityEngine;

namespace Framework.Audio
{
    /// <summary>
    /// Structure and properties for a sound effect
    /// </summary>
    [Serializable]
    public class SoundEffect : MonoBehaviour
    {
        // TODO :: consider making the Sound Effect multifaceted / multifunctional
        // meaning you can add a sound effect as a monobehaviour to do other functions
        // like allow a sound effect play a sound or respond to events
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float originalVolume;
        [SerializeField] private float duration;
        [SerializeField] private float playbackPosition;
        [SerializeField] private float time;
        [SerializeField] private Action callback;
        [SerializeField] private bool singleton;

        /// <summary>
        /// Gets or sets the name of the sound effect.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return audioSource.clip.name; }
        }

        /// <summary>
        /// Gets the length of the sound effect in seconds.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get { return audioSource.clip.length; }
        }

        /// <summary>
        /// Gets the playback position in seconds.
        /// </summary>
        /// <value>The playback position.</value>
        public float PlaybackPosition
        {
            get { return audioSource.time; }
        }

        /// <summary>
        /// Gets or sets the source of the sound effect.
        /// </summary>
        /// <value>The source.</value>
        public AudioSource Source
        {
            get { return audioSource; }
            set { audioSource = value; }
        }

        /// <summary>
        /// Gets or sets the original volume of the sound effect.
        /// </summary>
        /// <value>The original volume.</value>
        public float OriginalVolume
        {
            get { return originalVolume; }
            set { originalVolume = value; }
        }

        /// <summary>
        /// Gets or sets the duration for the sound effect to play in seconds.
        /// </summary>
        /// <value>The duration.</value>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// Gets or sets the time left or remaining for the sound effect to play in seconds.
        /// </summary>
        /// <value>The duration.</value>
        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Gets the normalised time left for the sound effect to play.
        /// </summary>
        /// <value>The normalised time.</value>
        public float NormalisedTime
        {
            get { return Time / Duration; }
        }

        /// <summary>
        /// Gets or sets the callback that would fire when the sound effect finishes playing.
        /// </summary>
        /// <value>The callback.</value>
        public Action Callback
        {
            get { return callback; }
            set { callback = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SoundEffect"/> is a singleton.
        /// Meaning that only one instance of the sound effect is ever allowed to be active.
        /// </summary>
        /// <value><c>true</c> if repeat; otherwise, <c>false</c>.</value>
        public bool Singleton
        {
            get { return singleton; }
            set { singleton = value; }
        }
    }
}