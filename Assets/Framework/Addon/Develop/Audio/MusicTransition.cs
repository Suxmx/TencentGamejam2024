namespace Framework.Audio
{
    /// <summary>
    /// What kind of music transition effect is going to take place
    /// </summary>
    public enum MusicTransition
    {
        /// <summary>
        /// (None) Immediately play the next one
        /// </summary>
        Swift,

        /// <summary>
        /// (In and Out) Fades out the current music then fades in the next one
        /// </summary>
        LinearFade,

        /// <summary>
        /// (No silent gaps) Smooth transition from current music to next
        /// </summary>
        CrossFade
    }
}