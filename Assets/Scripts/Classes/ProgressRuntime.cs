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

        public void LogProgressComplete(string levelProgressionName, string levelName, IScoreReport scoreReport)
        {
            if (_progressLogs == null) _progressLogs = new List<ProgressLog>();
            var log = new ProgressLog(levelProgressionName, levelName, scoreReport);
            Debug.Log(log);
            _progressLogs.Add(log);
        }

        public IProgressLog GetLastProgress()
        {
            if (_progressLogs == null || _progressLogs.Count == 0) return null;
            return _progressLogs[_progressLogs.Count - 1];
        }

        public string CurrentLevelProgression()
        {
            IProgressLog lastLog = GetLastProgress();
            return lastLog.LevelProgressionName;
        }

        public string CurrentLevel()
        {
            IProgressLog lastLog = GetLastProgress();
            return lastLog.LevelName;
        }

        public IProgressLog GetLastCompletedProgress()
        {
            if (_progressLogs == null || _progressLogs.Count == 0) return null;

            // Iterate backwards to find the last completed progress
            for (int i = _progressLogs.Count - 1; i >= 0; i--)
            {
                if (_progressLogs[i].ProgressType == EProgressLogType.Complete)
                {
                    return _progressLogs[i];
                }
            }

            return null;
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
        public IScoreReport ScoreReport { get; private set; }

        public ProgressLog(string levelProgressionName, string levelName, EProgressLogType progressType)
        {
            LevelProgressionName = levelProgressionName;
            LevelName = levelName;
            Timestamp = DateTime.UtcNow;
            ProgressType = progressType;
        }
        public ProgressLog(string levelProgressionName, string levelName, IScoreReport scoreReport)
        {
            LevelProgressionName = levelProgressionName;
            LevelName = levelName;
            Timestamp = DateTime.UtcNow;
            ProgressType = EProgressLogType.Complete;
            ScoreReport = scoreReport;
        }

        public override string ToString()
        {
            string scoreInfo = ScoreReport != null
                ? $", Score: {ScoreReport.TotalScore}, MaxCombo: {ScoreReport.MaxCombo}"
                : "";

            return $"ProgressLog({ProgressType}, Progression: {LevelProgressionName}, Level: {LevelName}, Time: {Timestamp:yyyy-MM-dd HH:mm:ss}{scoreInfo})";
        }
    }
}