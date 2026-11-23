namespace SoundSystem
{
    public class PlaySettings
    {
        public float time = 0;
        public float emergencyTime = 0f;
        public bool global = false;
        public float volumeLevel = 1;
        public int priority = 0;
        public bool stopLowerPriority = false;
        public bool mustBeHighestPriority = false;
        public bool looping;
        public bool persistent = false;
    }
}
