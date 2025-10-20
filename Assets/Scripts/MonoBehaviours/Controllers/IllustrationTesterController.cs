using UnityEngine;
using SwapPuzzle.Utilities;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SwapPuzzle.MonoBehaviours
{
    public class IllustrationTesterController : MonoBehaviour
    {
        [SerializeField] IllustrationViewer viewer;
        [SerializeField] Button fileRefreshButton;
        [SerializeField] TMP_Dropdown fileSelectorDropdown;
        [SerializeField] Button fileLoadButton;

        [SerializeField] TMP_InputField GridSizeInputField;
        [SerializeField] TMP_InputField PresolvedPieceCountInputField;

        private FilesystemSpriteLoader _spriteLoader;
        private IllustrationTesterLevelData _levelData = new();

        void Awake()
        {
            _spriteLoader = new FilesystemSpriteLoader();
            OnClickRefreshFileList();
        }

        private void RefreshLevelData(Sprite sprite)
        {
            IllustrationTesterIllustrationData illustrationData = new(sprite);
            _levelData.Illustration = illustrationData;

            RefreshLevelData();
        }

        private void RefreshLevelData()
        {
            int.TryParse(GridSizeInputField.text, out int gridSizeCount);
            _levelData.GridSize = gridSizeCount;

            int.TryParse(PresolvedPieceCountInputField.text, out int presolvedPiecesCount);
            _levelData.PresolvedPieces = presolvedPiecesCount;

            viewer.InitializePuzzle(_levelData);
        }

        public void OnClickRefreshFileList()
        {
            fileSelectorDropdown.ClearOptions();
            string[] paths = _spriteLoader.GetAllPngFiles();
            string folderPath = _spriteLoader.GetIllustrationsPath();

            List<string> options = new();
            for (int i = 0; i < paths.Length; i++)
            {
                string fileName = paths[i].Split(folderPath)[1];
                options.Add(fileName);
            }

            fileSelectorDropdown.AddOptions(options);
        }

        public void OnClickLoadSprite()
        {
            string currentlySelectedFile = fileSelectorDropdown.options[fileSelectorDropdown.value].text;
            currentlySelectedFile = _spriteLoader.GetIllustrationsPath() + currentlySelectedFile;
            Sprite sprite = _spriteLoader.LoadSprite(currentlySelectedFile);

            RefreshLevelData(sprite);
        }

        public void OnClickApplyGridSize()
        {
            RefreshLevelData();
        }

        public void OnClickApplyPresolvedPiece()
        {

            RefreshLevelData();
        }

        public void OnClickMix()
        {
            viewer.ShufflePieces();
        }

        public void OnClickSolve()
        {
            viewer.SolvePieces();
        }
    }
}