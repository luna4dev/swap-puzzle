using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IObjectPool<T> where T : MonoBehaviour
    {
        T Get();
        void Return(T obj);
        void Prewarm(int count);
    }
}