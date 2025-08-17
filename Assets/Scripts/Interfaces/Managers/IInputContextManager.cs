using System;
using System.Collections.Generic;

namespace SwapPuzzle.Interfaces
{
    public interface IInputContextManager
    {
        event Action OnContextChanged;
        bool IsEnabled { get; }
        IInputContext CurrentContext { get; }

        void PushContext(IInputContext context);
        bool PopContext();
        void ClearAllContexts();

        bool ContainsContext(IInputContext context);
        bool ContainsContext<T>() where T : class, IInputContext;
        T GetContext<T>() where T : class, IInputContext;
        IEnumerable<IInputContext> GetAllContexts();

        bool ProcessInput(InputType inputType, InputData inputData);
    }
}