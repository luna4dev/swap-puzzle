namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Manages the book-style viewing of illustrations
    /// </summary>
    public interface IBookViewer
    {
        /// <summary>
        /// Opens the book viewer
        /// </summary>
        void OpenBook();

        /// <summary>
        /// Navigates to a specific page in the book
        /// </summary>
        /// <param name="pageNumber">The page number to navigate to</param>
        void NavigateToPage(int pageNumber);

        /// <summary>
        /// Shows an illustration in fullscreen mode
        /// </summary>
        /// <param name="illustrationId">The ID of the illustration to show</param>
        void ShowFullscreenIllustration(int illustrationId);

        /// <summary>
        /// Closes the book viewer
        /// </summary>
        void CloseBook();
    }
} 