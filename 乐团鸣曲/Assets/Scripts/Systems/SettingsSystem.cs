using System.IO;
using UnityEngine;
namespace YuetanMingqu
{

    public static class SettingsSystem
    {
        private const string FOLDER_NAME = "ResonanceShelter";
        private const string FILE_NAME = "settings.json";

        public static SettingsData Current {  get; private set; }

        private static string FilePath => Path.Combine(Application.persistentDataPath, FOLDER_NAME, FILE_NAME);

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                Current = JsonUtility.FromJson<SettingsData>(json);
                if (Current == null)
                {
                    Current = new SettingsData();
                    Current.ResetToDefault();
                }
            }
            else
            {
                Current = new SettingsData();
                Current.ResetToDefault(); 
            }
        }

        public static void Save()
        {
            if (Current == null) return;
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            string json = JsonUtility.ToJson(Current);
            File.WriteAllText(FilePath,json);
        }
    }

}
