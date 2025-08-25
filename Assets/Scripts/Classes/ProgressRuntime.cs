using SwapPuzzle.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Classes
{
    [Serializable]
    public class ProgressRuntime : IProgressRuntime, ISaveLoadable
    {
        [SerializeField] private List<ProgressLog> _progressLogs = new List<ProgressLog>();
        public List<IProgressLog> ProgressLogs => new List<IProgressLog>(_progressLogs != null ? _progressLogs : new List<ProgressLog>());

        public ProgressRuntime(string serializedString)
        {
            InitializeFromSerializedString(serializedString);
        }

        public void LogProgressStart(ILevelProgressionData levelProgressionData, ILevelData levelData)
        {
            if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            var log = new ProgressLog(levelProgressionData.Name, levelData.Name, EProgressLogType.Start);
            _progressLogs.Add(log);
        }

        public void LogProgressComplete(ILevelProgressionData levelProgressionData, ILevelData levelData)
        {
            if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            var log = new ProgressLog(levelProgressionData.Name, levelData.Name, EProgressLogType.Complete);
            _progressLogs.Add(log);
        }

        public ProgressLog Last()
        {
            if (_progressLogs.Count == 0) return null;
            return _progressLogs[^1];
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public void InitializeFromSerializedString(string serializedString)
        {
            if (string.IsNullOrEmpty(serializedString))
            {
                _progressLogs = new List<ProgressLog>();
                return;
            }
            
            try
            {
                JsonUtility.FromJsonOverwrite(serializedString, this);
                if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            }
            catch
            {
                _progressLogs = new List<ProgressLog>();
            }
        }
    }

    [Serializable]
    public class ProgressLog : IProgressLog
    {
        public string LevelProgressionName { get; private set; }
        public string LevelName { get; private set; }
        public DateTime Timestamp { get; private set; }
        public EProgressLogType ProgressType { get; private set; }

        public ProgressLog(string levelProgressionName, string levelName, EProgressLogType progressType)
        {
            LevelProgressionName = levelProgressionName;
            LevelName = levelName;
            Timestamp = DateTime.UtcNow;
            ProgressType = progressType;
        }
    }
}