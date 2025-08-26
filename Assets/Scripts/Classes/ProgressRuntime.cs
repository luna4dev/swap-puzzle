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

        /// <summary>
        /// fresh new progress runtime
        /// </summary>
        public ProgressRuntime() { }

        /// <summary>
        /// loading saved progress runtime
        /// </summary>
        /// <param name="serializedString"></param>
        public ProgressRuntime(string serializedString)
        {
            InitializeFromSerializedString(serializedString);
        }

        public void LogProgressStart(string levelProgressionName, string levelName)
        {
            if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            var log = new ProgressLog(levelProgressionName, levelName, EProgressLogType.Start);
            _progressLogs.Add(log);
        }

        public void LogProgressComplete(string levelProgressionName, string levelName)
        {
            if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            var log = new ProgressLog(levelProgressionName, levelName, EProgressLogType.Complete);
            _progressLogs.Add(log);
        }

        public string CurrentLevelProgression()
        {
            if (_progressLogs == null || _progressLogs.Count == 0) return null;
            
            var lastLog = _progressLogs[_progressLogs.Count - 1];
            return lastLog.LevelProgressionName;
        }

        public string CurrentLevel()
        {
            if (_progressLogs == null || _progressLogs.Count == 0) return null;
            
            var lastLog = _progressLogs[_progressLogs.Count - 1];
            return lastLog.LevelName;
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