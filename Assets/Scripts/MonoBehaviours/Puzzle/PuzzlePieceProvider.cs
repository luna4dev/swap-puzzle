using System.Collections.Generic;
using SwapPuzzle.Interfaces;
using UnityEngine;

namespace SwapPuzzle.MonoBehaviours
{
    public class PuzzlePieceProvider : MonoBehaviour, IObjectPool<PuzzlePiece>
    {
        [SerializeField] private PuzzlePiece piecePrefab;
        Stack<PuzzlePiece> puzzlePiecePool = new();

        private void Prepare()
        {
            var newObj = Instantiate(piecePrefab, transform);
            newObj.gameObject.SetActive(false);
            puzzlePiecePool.Push(newObj);
        }

        public PuzzlePiece Get()
        {
            if (puzzlePiecePool.Count == 0) Prepare();
            PuzzlePiece obj = puzzlePiecePool.Pop();

            // prepare before returning
            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Return(PuzzlePiece obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform, false);
            puzzlePiecePool.Push(obj);
        }

        public void Prewarm(int count)
        {
            while (puzzlePiecePool.Count < count)
            {
                Prepare();
            }
        }
    }
}