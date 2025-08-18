using System.Threading.Tasks;

namespace SwapPuzzle.Interfaces
{
    public interface IPopupController
    {
        /// <summary>
        /// open popup by popup name
        /// </summary>
        /// <returns></returns>
        Task<T> OpenPopup<T>() where T : IPopup;

        /// <summary>
        /// close popup by reference
        /// this should be called only by the popup instance to close itself
        /// </summary>
        /// <param name="popup"></param>
        void ClosePopup(IPopup popup);

        /// <summary>
        /// close all popup
        /// </summary>
        void CloseAllPopup();
    }
} 