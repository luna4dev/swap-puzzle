using System.Collections.Generic;
using UnityEngine;

namespace SwapPuzzle.Interfaces
{
    public interface IInputContextManager
    {
        IInputContext CurrentContext { get; }
        int ContextCount { get; }
        bool HasContexts { get; }
        
        void PushContext(IInputContext context);
        bool PopContext();
        bool PopContext(IInputContext context);
        void ClearAllContexts();
        
        bool ContainsContext(IInputContext context);
        bool ContainsContext<T>() where T : class, IInputContext;
        T GetContext<T>() where T : class, IInputContext;
        IEnumerable<IInputContext> GetAllContexts();
        
        bool ProcessInput(InputType inputType, InputData inputData);
        
        void SetEnabled(bool enabled);
        bool IsEnabled { get; }
        
        event System.Action<IInputContext> OnContextPushed;
        event System.Action<IInputContext> OnContextPopped;
        event System.Action OnContextsCleared;
    }
}