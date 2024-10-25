using System;

namespace Framework
{
    public interface IProcedureManager : IService
    {
        public ProcedureBase CurrentProcedure { get; }
        public void SetValue(string name, object value);
        public T GetValue<T>(string name);
        public bool TryGetValue<T>(string name, out T value);

    }
}