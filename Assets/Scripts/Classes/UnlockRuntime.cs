using SwapPuzzle.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Classes
{
    [Serializable]
    public class UnlockRuntime : IUnlockRuntime, ISaveLoadable
    {
        [SerializeField] private List<UnlockLog> _unlockLogs = new List<UnlockLog>();
        public List<IUnlockLog> UnlockLogs => new List<IUnlockLog>(_unlockLogs ?? new List<UnlockLog>());

        public UnlockRuntime(string serializedString) {
            InitializeFromSerializedString(serializedString);
        }

        public void LogUnlock(IIllustrationData data)
        {
            if (_unlockLogs == null) _unlockLogs = new List<UnlockLog>();
            UnlockLog log = new(data.Name);
            _unlockLogs.Add(log);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public void InitializeFromSerializedString(string serializedString)
        {
            if (string.IsNullOrEmpty(serializedString))
            {
                _unlockLogs = new List<UnlockLog>();
                return;
            }
            
            try
            {
                JsonUtility.FromJsonOverwrite(serializedString, this);
                _unlockLogs ??= new List<UnlockLog>();
            }
            catch
            {
                _unlockLogs = new List<UnlockLog>();
            }
        }
    }

    [Serializable]
    public class UnlockLog : IUnlockLog
    {
        public string IllustrationName { get; private set; }
        public DateTime Timestamp { get; private set; }

        public UnlockLog(string illustrationName)
        {
            IllustrationName = illustrationName;
            Timestamp = DateTime.UtcNow;
        }
    }

}