namespace SwapPuzzle.Interfaces
{
    /// <summary>
    /// Controls the index/gallery scene functionality
    /// </summary>
    public interface IIndexController : ISceneController
    {
        /// <summary>
        /// Displays the illustration gallery
        /// </summary>
        void DisplayGallery();

        /// <summary>
        /// Handles selection of an illustration
        /// </summary>
        /// <param name="illustrationId">The ID of the selected illustration</param>
        void OnIllustrationSelected(int illustrationId);

        /// <summary>
        /// Returns to the main menu
        /// </summary>
        void ReturnToMainMenu();
    }
} 