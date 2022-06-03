using System;

namespace SHACAL.interfaces
{
    public interface IRoundTranformation
    {
        public UInt32 RoundFunction(UInt32 numberBlockB, UInt32 numberBlockC, UInt32 numberBlockD, int round);
    }
}