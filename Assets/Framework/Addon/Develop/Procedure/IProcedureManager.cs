using System;

namespace Framework
{
    public interface IProcedureManager : IService
    {
        public ProcedureBase CurrentProcedure { get; }
    }
}