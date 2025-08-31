namespace SwapPuzzle.Interfaces
{
    public interface IPopup: IInputContext
    {
        /// <summary>
        /// Lifecycle call when popup is opened.
        /// it is called whenever popup is opened
        /// </summary>
        void InitializePopup();
        void ClosePopup();
    }
} 