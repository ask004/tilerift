using System.IO;
using UnityEngine;

namespace TileRift.Runtime
{
    public sealed class ProgressRepository
    {
        private readonly string _path;

        public ProgressRepository(string customPath = null)
        {
            _path = string.IsNullOrWhiteSpace(customPath)
                ? Path.Combine(Application.persistentDataPath, "player_progress.json")
                : customPath;
        }

        public PlayerProgress Load()
        {
            if (!File.Exists(_path))
            {
                return PlayerProgress.Default();
            }

            var json = File.ReadAllText(_path);
            var data = JsonUtility.FromJson<PlayerProgress>(json);
            return data ?? PlayerProgress.Default();
        }

        public void Save(PlayerProgress progress)
        {
            var json = JsonUtility.ToJson(progress, prettyPrint: true);
            File.WriteAllText(_path, json);
        }
    }
}
