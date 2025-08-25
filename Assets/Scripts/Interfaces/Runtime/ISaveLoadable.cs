namespace SwapPuzzle.Interfaces
{
    public enum ERuntimeType
    {
        Progress,
        Unlock
    }
    public interface ISaveLoadable
    {
        string Serialize();
        void InitializeFromSerializedString(string serializedString);
    }
}