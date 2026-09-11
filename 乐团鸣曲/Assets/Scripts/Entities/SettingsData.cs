namespace YuetanMingqu
{
    [System.Serializable]
    public class SettingsData
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public void ResetToDefault()
        {
            masterVolume = 1.0f;
            musicVolume = 0.8f;
            sfxVolume = 0.8f;
        }
    }

}

